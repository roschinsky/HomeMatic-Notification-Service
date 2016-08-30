using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml;
using TRoschinsky.Lib.HomeMaticXmlApi;

namespace TRoschinsky.Service.HomeMaticNotification
{
    /// <summary>
    /// The HMNotifier class is the heart of the HomeMaticNotification service. After initialization 
    /// like reading the config and establishing the connection to the Homematic CCU2, it collects 
    /// all events of HMNotifyItems by calling the CollectEvents() method. It'll compare them 
    /// to their last recognized state, check if a notification should be send out and sends it to 
    /// the defined HMNotifyDestinations.
    /// state
    /// </summary>
    public class HMNotifier : IDisposable
    {
        private string pushLink = String.Empty;
        private string pushSource = String.Empty;
        private string pushTitle = "HMC";
        private string configFileName = String.Empty;
        private Tuple<string, string, string, string, string> mailConfig;

        private HMApiWrapper hmWrapper;
        private BackgroundWorker bgwGetEvents;
        private TimeSpan delayFullReqeusts;
        private DateTime lastFullRequest;

        private bool justSimulateSending = false;
        public bool JustSimulateSending { get { return justSimulateSending; } set { justSimulateSending = value; } }

        public bool IsConnected { get; private set; }
        public long WorkerRunCount { get; private set; }
        public long WorkerTriggeredCount { get; private set; }

        private List<Exception> errors = new List<Exception>();
        private List<HMNotification> notifications = new List<HMNotification>();
        public HMNotification[] Notifications { get { return notifications.ToArray(); } }
        
        private List<HMNotifyItem> notifyItems;
        public List<HMNotifyItem> NotifyItems { get { return notifyItems; } }

        private List<HMNotifyDestination> notifyDestinations = new List<HMNotifyDestination>();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="ccuUrl">The URL to the Homematic CCU2</param>
        /// <param name="delayTillNextFullReqeust">Time intervall in seconds until a full scan should be executed</param>
        public HMNotifier(string ccuUrl, string delayTillNextFullReqeust, string configFile)
        {
            hmWrapper = null;
            pushSource = ccuUrl;
            configFileName = configFile;
            WorkerRunCount = WorkerTriggeredCount = 0;

            delayFullReqeusts = new TimeSpan(0, 0, int.Parse(delayTillNextFullReqeust));

            Tuple<List<HMNotifyDestination>, List<HMNotifyItem>> config = ReadConfig();
            notifyDestinations = config.Item1;
            notifyItems = config.Item2;

            IsConnected = ConnectToHmApi(ccuUrl);

            if (IsConnected)
            {
                // Setup BackgroundWorker
                bgwGetEvents = new BackgroundWorker();
                bgwGetEvents.DoWork += bgwGetEvents_DoWork;
                bgwGetEvents.RunWorkerCompleted += bgwGetEvents_RunWorkerCompleted;

                // Setup high priorized notify items
                foreach(HMNotifyItem notifyItem in notifyItems.Where(ni => ni.IsImportant))
                {
                    hmWrapper.FastUpdateDeviceSetup(notifyItem.DeviceAddress);
                }

                // initial run
                CollectEvents();
            }
            else
            {
                Dispose();
            }
        }

        public HMNotifier(HMNotifierConfig config) 
            : this(config.HmcUrl, config.NotifierQueryFullRequestSec, config.NotifierConfigFile)
        {
            mailConfig = new Tuple<string, string, string, string, string>(
                config.NotificationSmtpHost,
                config.NotificationSmtpPort,
                config.NotificationSmtpCredUser,
                config.NotificationSmtpCredPw,
                config.NotificationSmtpMailFrom);
        }

        /// <summary>
        /// Triggers an async job to collect all status changes of configured HMNotifyItems
        /// </summary>
        public void CollectEvents()
        {
            try
            {
                WorkerTriggeredCount++;
                if (IsConnected && !bgwGetEvents.IsBusy)
                {
                    bgwGetEvents.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }
        }

        /// <summary>
        /// Async job to collect collect all status changes of configured HMNotifyItems.
        /// This method is the main business logic. It handles the updates executed by the Homematic API, 
        /// checks for changes and relevance and sends notifications if necessary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bgwGetEvents_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkerRunCount++;

            if (delayFullReqeusts < (DateTime.Now - lastFullRequest))
            {
                hmWrapper.UpdateStates(false);
                hmWrapper.UpdateVariables();
                lastFullRequest = DateTime.Now;
            }
            else
            {
                hmWrapper.UpdateStates(true);
            }

            string currentnotifyItemName = string.Empty;
            List<HMNotification> result = new List<HMNotification>();

            try
            {
                // iterate all configured HMNotifyItems
                for (int i = 0; i < notifyItems.Count; i++ )
                {
                    try
                    {
                        currentnotifyItemName = notifyItems[i].ToString();

                        // check if there was a last notification for current notify item and if it was logged a time span greater than query interval ago
                        if (notifyItems[i].LastNotification == null || (DateTime.Now - notifyItems[i].LastNotification.TimeStamp) > notifyItems[i].QueryIntervall)
                        {
                            HMNotification currentNotification = null;
                            string fullAddress = String.Empty;

                            if (notifyItems[i].HMNotifyType == HMNotifyItem.ItemType.Device)
                            {
                                fullAddress = String.Format("{0}:{1}", notifyItems[i].DeviceAddress, notifyItems[i].DeviceChannel);

                                // ask Homematic wrapper for datapoint to check for...
                                HMDeviceDataPoint notifyDataPoint = hmWrapper.GetDataByAddress(fullAddress, notifyItems[i].ValueKey);

                                // check if datapoint was obtained properly; if not throw an error
                                if (notifyDataPoint == null)
                                {
                                    throw new HMNException(String.Format("DataPoint for notify item '{0}' with address {1} could not be checked. Please check configuration!", notifyItems[i].Name, fullAddress), null);
                                }

                                // create new notification item
                                currentNotification = new HMNotification()
                                {
                                    Scope = notifyItems[i].Scope,
                                    Name = notifyItems[i].Name,
                                    Address = fullAddress,
                                    DataPoint = notifyDataPoint,
                                    NotificationSent = false
                                };
                            }
                            else if(notifyItems[i].HMNotifyType == HMNotifyItem.ItemType.Variable)
                            {
                                // ask Homematic wrapper for system variable to check for...
                                HMSystemVariable notifyVariable = hmWrapper.Variables.First(v => v.InternalId == notifyItems[i].VariableId);

                                // check if system variable was obtained properly; if not throw an error
                                if (notifyVariable == null)
                                {
                                    throw new HMNException(String.Format("System variable for notify item '{0}' with address {1} could not be checked. Please check configuration!", notifyItems[i].Name, String.Concat(notifyItems[i].VariableId)), null);
                                }

                                // create new notification variable
                                currentNotification = new HMNotification()
                                {
                                    Name = notifyItems[i].Name,
                                    Address = String.Concat(notifyItems[i].VariableId),
                                    Variable = notifyVariable,
                                    NotificationSent = false
                                };
                            }
                            else
                            {
                                throw new HMNException(String.Format("Notify item '{0}' was not properly defined. Please check configuration!", notifyItems[i].Name), null);
                            }

                            // set last notification of current notify item if it is not set...
                            if (notifyItems[i].LastNotification == null)
                            {
                                notifyItems[i].LastNotification = currentNotification;
                            }
                            // ...or checks for a change in status of the current notify item
                            //else if (notifyItems[i].LastNotification.DataPoint.ValueString != null
                            //    && currentNotification.DataPoint.ValueString != null
                            //    && notifyItems[i].LastNotification.DataPoint.ValueString != currentNotification.DataPoint.ValueString)
                            else if (WasChangedSinceLastCheck(notifyItems[i].LastNotification, currentNotification))
                            {
                                notifyItems[i].LastNotification = currentNotification;

                                // Check if we need to prevent from notifying due to a silence time
                                bool noSilenceTime = notifyItems[i].SendNotification;

                                // Check if we need to prevent from notifying due to a state was changed to a state, that is not worth a push notification
                                bool noStatusBlocker = (String.IsNullOrEmpty(notifyItems[i].PreventNotificationStatus) || 
                                    String.IsNullOrEmpty(currentNotification.DataPoint.ValueString) || 
                                    currentNotification.DataPoint.ValueString.ToLower() != notifyItems[i].PreventNotificationStatus.ToLower());

                                // Check if we need to prevent from notifying when not matching a given condition, so we do not need to send a push notification
                                bool noConditionMismatch = true;
                                foreach(HMNotifyCondition condition in notifyItems[i].Conditions)
                                {
                                    bool varMismatch = false;
                                    bool devMismatch = false;

                                    if(condition.CType == HMNotifyCondition.ConditionType.Variable)
                                    {
                                        HMSystemVariable variable = hmWrapper.Variables.First(v => v.InternalId == condition.IseId);
                                        if(variable != null)
                                        {
                                            varMismatch = condition.ConditionMatchValue.ToLower() != variable.ValueString.ToLower();
                                        }
                                    }
                                    else if(condition.CType == HMNotifyCondition.ConditionType.Device)
                                    {
                                        HMDeviceDataPoint dataPoint = hmWrapper.GetDataByAddress(condition.DeviceAddress, condition.ValueKey);
                                        if(dataPoint != null)
                                        {
                                            devMismatch = condition.ConditionMatchValue.ToLower() != dataPoint.ValueString.ToLower();
                                        }
                                    }

                                    if(varMismatch || devMismatch)
                                    {
                                        noConditionMismatch = false;
                                    }
                                }

                                // decide whether the notifications should be sent or not
                                if (noSilenceTime && noStatusBlocker && noConditionMismatch)
                                {
                                    HMNotifyDestination destination = notifyDestinations.First(nd => nd.Id == notifyItems[i].SendNotificationTo);
                                    if(destination == null)
                                    {
                                        throw new HMNException(String.Format("No destination for #{0} found...", notifyItems[i].SendNotificationTo));
                                    }

                                    if (justSimulateSending)
                                    {
                                        // just pretend to send a message (for debugging purpose)
                                        currentNotification.NotificationSent = true;
                                        System.Diagnostics.Debug.WriteLine(String.Format("Would send '{0}' to {1}", currentNotification, destination));
                                    }
                                    else
                                    {
                                        // send a message by the specified notification destination
                                        currentNotification.NotificationSent = SendNotification(currentNotification, destination, notifyItems[i].IsImportant, notifyItems[i].IsSilent, NotifyItems[i].HMNotifyType);
                                    }
                                }

                                result.Add(currentNotification);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        errors.Add(new HMNException(currentnotifyItemName, ex));
                    }
                }

                e.Result = result;
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }
        }

        private bool WasChangedSinceLastCheck(HMNotification lastNotification, HMNotification currentNotification)
        {
            try
            {
                // Check for changed device status
                if (currentNotification.DataPoint != null
                    && lastNotification.DataPoint.ValueString != null
                    && currentNotification.DataPoint.ValueString != null
                    && lastNotification.DataPoint.ValueString != currentNotification.DataPoint.ValueString)
                {
                    return true;
                }

                // Check for changed variable status
                if (currentNotification.Variable != null
                    && lastNotification.Variable.ValueString != null
                    && currentNotification.Variable.ValueString != null
                    && lastNotification.Variable.ValueString != currentNotification.Variable.ValueString)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            return false;
        }

        /// <summary>
        /// Async job finished to collect all status changes of configured HMNotifyItems
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bgwGetEvents_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                List<HMNotification> loggedNotifications = e.Result as List<HMNotification>;
                if (!e.Cancelled && loggedNotifications != null && loggedNotifications.Count > 0)
                {
                    notifications.AddRange(loggedNotifications);
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }
        }

        /// <summary>
        /// Sends a push or mail notification to the configured notify-provider
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="destination"></param>
        /// <param name="isImportant"></param>
        /// <param name="isSilent"></param>
        private bool SendNotification(HMNotification notification, HMNotifyDestination destination, bool isImportant, bool isSilent, HMNotifyItem.ItemType notifyType)
        {
            try
            {
                string title = String.Format("{0} - {2}: {1}", pushTitle, notification.Name, notifyType == HMNotifyItem.ItemType.Device ? notification.Scope : "System Variable");
                string message = String.Empty;

                if (notifyType == HMNotifyItem.ItemType.Device)
                {
                    string channelDetails = String.Empty;
                    HMDeviceChannel channel = hmWrapper.GetChannelByAddress(notification.Address);
                    if (channel != null)
                    {
                        channelDetails = String.Format("** Details '{0}' **\n", channel.Name);
                        foreach (KeyValuePair<string, HMDeviceDataPoint> dataPoint in channel.DataPoints)
                        {
                            channelDetails += String.Format("{0}: {1}\n", dataPoint.Key, dataPoint.Value);
                        }
                    }

                    message = String.Format("Ereignis in {0} für {1} mit Status '{2}' eingetreten.\nAusgelöst um: '{3}'\nSensor/Aktor: {4}\n\n{5}",
                                            notification.Scope,
                                            notification.Name,
                                            notification.DataPoint.Value,
                                            notification.TimeStamp,
                                            notification.Address,
                                            channelDetails);
                }
                else if(notifyType == HMNotifyItem.ItemType.Variable)
                {
                    string variableDetails = String.Empty;
                    HMSystemVariable sysVar = notification.Variable;
                    if (sysVar != null)
                    {
                        variableDetails = String.Format("** Details '{0}' **\n", sysVar.Name);
                        variableDetails += String.Format("VALUE: {0}\n", sysVar.ValueDescription);
                        variableDetails += String.Format("VALUE-TYPE: {0}\n", sysVar.ValueType);
                        variableDetails += String.Format("VALUE-UNIT: {0}\n", sysVar.ValueUnit);
                    }

                    message = String.Format("Variable {0} auf Status '{1}' geändert.\nGeändert um: '{2}'\nInterne Variablen-ID #{3}\n\n{4}",
                                            notification.Name,
                                            notification.Variable.Value,
                                            notification.TimeStamp,
                                            notification.Address,
                                            variableDetails);
                }
                else
                {
                    message = notification.ToString();
                }

                Notification notify = null;

                if (destination.NotifyProvider == HMNotifyDestination.NotifyProviderType.Pushalot)
                {
                    notify = new NotificationPushalot(destination.DestinationAddress, message, title, isImportant, isSilent);
                    ((NotificationPushalot)notify).Link = pushLink;
                    ((NotificationPushalot)notify).Source = destination.Name;
                    return notify.Send();
                }
                else if(destination.NotifyProvider == HMNotifyDestination.NotifyProviderType.Pushover)
                {
                    notify = new NotificationPushover(destination.DestinationAddress, message, title, isImportant, isSilent);
                    return notify.Send();
                }
                else if(destination.NotifyProvider == HMNotifyDestination.NotifyProviderType.Telegram)
                {
                    notify = new NotificationTelegram(destination.DestinationAddress, message, title, isImportant, isSilent);
                    return notify.Send();
                }
                else if(destination.NotifyProvider == HMNotifyDestination.NotifyProviderType.Email)
                {
                    notify = new NotificationSmtp(destination.DestinationAddress, message, title, isImportant, isSilent);
                    ((NotificationSmtp)notify).SmtpConfig = mailConfig;
                    ((NotificationSmtp)notify).Link = pushLink;
                    ((NotificationSmtp)notify).Source = destination.Name;
                    return notify.Send();
                }
                else
                {
                    throw new HMNException("There was no notify-provider defined in destination.", null);
                }
            }
            catch(Exception ex)
            {
                errors.Add(ex);
            }
            return false;
        }

#region Helper

        /// <summary>
        /// Connects to HomeMatic XML-API
        /// </summary>
        /// <param name="homeMaticUrl">Url to HomeMatic</param>
        /// <returns>True if succeeded</returns>
        private bool ConnectToHmApi(string homeMaticUrl)
        {
            try
            {
                hmWrapper = new HMApiWrapper(new Uri(homeMaticUrl), true, true);
                lastFullRequest = DateTime.Now;
                return true;
            }
            catch(Exception ex)
            {
                errors.Add(ex);
            }
            return false;
        }

        /// <summary>
        /// Read configuration file and builds internal data structure
        /// </summary>
        /// <returns>Tuple with list of NotifyDestinations and NotifyItems related to them</returns>
        private Tuple<List<HMNotifyDestination>, List<HMNotifyItem>> ReadConfig()
        {
            List<HMNotifyDestination> hmNotifyDestinations = new List<HMNotifyDestination>();
            List<HMNotifyItem> hmNotifyItems = new List<HMNotifyItem>();

            try
            {
                XmlNode config = GetConfigXml(configFileName, "configHMNotifier");

                if (config != null && config.FirstChild != null && config.FirstChild.Name == "notifyGroups" && config.FirstChild.ChildNodes != null)
                {
                    if (config.Attributes["pushMessageTitle"] != null)
                    {
                        pushTitle = config.Attributes["pushMessageTitle"].Value;
                    }

                    if (config.Attributes["pushMessageUrlToWebUi"] != null)
                    {
                        pushLink = config.Attributes["pushMessageUrlToWebUi"].Value;
                    }

                    // Get groups from config
                    foreach (XmlNode hmNotifyDestNode in config.FirstChild.ChildNodes)
                    {
                        try
                        {
                            HMNotifyDestination hmNotifyDest = new HMNotifyDestination() 
                            {
                                Id = int.Parse(hmNotifyDestNode.Attributes["Id"].Value),
                                Name = hmNotifyDestNode.Attributes["Name"].Value,
                                Owner = hmNotifyDestNode.Attributes["Owner"].Value
                            };

                            foreach (XmlNode itemNode in hmNotifyDestNode.ChildNodes)
                            {
                                if (!String.IsNullOrWhiteSpace(itemNode.InnerText) || itemNode.HasChildNodes)
                                {
                                    if (itemNode.Name == "pushalotApiKey")
                                    {
                                        hmNotifyDest.DestinationAddress = itemNode.InnerText;
                                        hmNotifyDest.NotifyProvider = HMNotifyDestination.NotifyProviderType.Pushalot;
                                        hmNotifyDestinations.Add(hmNotifyDest);
                                    }
                                    if (itemNode.Name == "pushoverApiKey")
                                    {
                                        hmNotifyDest.DestinationAddress = itemNode.InnerText;
                                        hmNotifyDest.NotifyProvider = HMNotifyDestination.NotifyProviderType.Pushover;
                                        hmNotifyDestinations.Add(hmNotifyDest);
                                    }
                                    if (itemNode.Name == "notifyBy")
                                    {
                                        string pushProvider = itemNode.Attributes["NotifyProvider"].Value;
                                        if(pushProvider.ToLower() == "pushalot")
                                        {
                                            hmNotifyDest.NotifyProvider = HMNotifyDestination.NotifyProviderType.Pushalot;
                                        }
                                        else if (pushProvider.ToLower() == "pushover")
                                        {
                                            hmNotifyDest.NotifyProvider = HMNotifyDestination.NotifyProviderType.Pushover;
                                        }
                                        else if (pushProvider.ToLower() == "telegram")
                                        {
                                            hmNotifyDest.NotifyProvider = HMNotifyDestination.NotifyProviderType.Telegram;
                                        }
                                        else if (pushProvider.ToLower() == "email")
                                        {
                                            hmNotifyDest.NotifyProvider = HMNotifyDestination.NotifyProviderType.Email;
                                        }
                                        hmNotifyDest.DestinationAddress = itemNode.InnerText;
                                        hmNotifyDestinations.Add(hmNotifyDest);
                                    }

                                    if (itemNode.Name == "notifyItems")
                                    {
                                        // Get notification items for notification group from config
                                        foreach (XmlNode hmItemXml in itemNode.ChildNodes)
                                        {
                                            try
                                            {
                                                HMNotifyItem hmNotifyItem = null;

                                                if (hmItemXml.Name.ToLower() == "notifyvariable" && hmItemXml.Attributes.GetNamedItem("IseId") != null)
                                                {
                                                    hmNotifyItem = new HMNotifyItem()
                                                    {
                                                        VariableId = int.Parse(hmItemXml.Attributes["IseId"].Value),
                                                        Name = hmItemXml.Attributes["Name"].Value
                                                    };
                                                }
                                                else if (hmItemXml.Name.ToLower() == "notifyitem" && hmItemXml.Attributes.GetNamedItem("DeviceAddress") != null)
                                                {
                                                    hmNotifyItem = new HMNotifyItem()
                                                    {
                                                        DeviceAddress = hmItemXml.Attributes["DeviceAddress"].Value,
                                                        DeviceChannel = hmItemXml.Attributes["DeviceChannel"].Value,
                                                        Scope = hmItemXml.Attributes["Scope"].Value,
                                                        Name = hmItemXml.Attributes["Name"].Value,
                                                        ValueKey = hmItemXml.Attributes["ValueKey"].Value
                                                    };
                                                }

                                                hmNotifyItem.IsImportant = bool.Parse(hmItemXml.Attributes["IsImportant"] != null ? hmItemXml.Attributes["IsImportant"].Value : "false");
                                                hmNotifyItem.IsSilent = bool.Parse(hmItemXml.Attributes["IsSilent"] != null ? hmItemXml.Attributes["IsSilent"].Value : "false");
                                                if (hmItemXml.Attributes["PreventNotificationStatus"] != null)
                                                {
                                                    hmNotifyItem.PreventNotificationStatus = hmItemXml.Attributes["PreventNotificationStatus"].Value;
                                                }
                                                if (hmItemXml.Attributes["QueryIntervallSeconds"] != null)
                                                {
                                                    hmNotifyItem.QueryIntervallSeconds = int.Parse(hmItemXml.Attributes["QueryIntervallSeconds"].Value);
                                                }

                                                // Check for sub-items like silence times and conditions
                                                char[] timeToken = new char[] {':'};
                                                foreach (XmlElement hmSubGroupNode in hmItemXml.ChildNodes)
                                                {
                                                    if (!hmSubGroupNode.IsEmpty)
                                                    {
                                                        if (hmSubGroupNode.Name == "silenceTimes")
                                                        {
                                                            foreach (XmlElement silenceNode in hmSubGroupNode.ChildNodes)
                                                            {
                                                                try
                                                                {
                                                                    string[] aStart = silenceNode.Attributes["Start"].Value.Split(timeToken, StringSplitOptions.None);
                                                                    string[] aEnd = silenceNode.Attributes["End"].Value.Split(timeToken, StringSplitOptions.None);

                                                                    DayOfWeek day = DayOfWeek.Monday;
                                                                    switch (silenceNode.Attributes["Day"].Value)
                                                                    {
                                                                        case "Monday":
                                                                            day = DayOfWeek.Monday;
                                                                            break;
                                                                        case "Tuesday":
                                                                            day = DayOfWeek.Tuesday;
                                                                            break;
                                                                        case "Wednesday":
                                                                            day = DayOfWeek.Wednesday;
                                                                            break;
                                                                        case "Thursday":
                                                                            day = DayOfWeek.Thursday;
                                                                            break;
                                                                        case "Friday":
                                                                            day = DayOfWeek.Friday;
                                                                            break;
                                                                        case "Saturday":
                                                                            day = DayOfWeek.Saturday;
                                                                            break;
                                                                        case "Sunday":
                                                                            day = DayOfWeek.Sunday;
                                                                            break;
                                                                        default:
                                                                            break;
                                                                    }

                                                                    HMNotifySilence silence = new HMNotifySilence(int.Parse(aStart[0]), int.Parse(aStart[1]), int.Parse(aEnd[0]), int.Parse(aEnd[1]), day);
                                                                    hmNotifyItem.AddSilenceTime(silence);
                                                                }
                                                                catch(Exception ex)
                                                                {
                                                                    errors.Add(ex);
                                                                }
                                                            }
                                                        }

                                                        if(hmSubGroupNode.Name == "conditions")
                                                        {
                                                            foreach (XmlElement conditionNode in hmSubGroupNode.ChildNodes)
                                                            {
                                                                try
                                                                {
                                                                    HMNotifyCondition condition = null;

                                                                    if (conditionNode.Name == "varCondition")
                                                                    {
                                                                        int iseId = int.Parse(conditionNode.Attributes["IseId"].Value);
                                                                        string matchValue = conditionNode.Attributes["ConditionMatchValue"].Value;
                                                                        condition = new HMNotifyCondition(iseId, matchValue);
                                                                    }
                                                                    else if (conditionNode.Name == "devCondition")
                                                                    {
                                                                        string devAddress = conditionNode.Attributes["DeviceAddress"].Value + ":" + conditionNode.Attributes["DeviceChannel"].Value;
                                                                        string devValueKey = conditionNode.Attributes["ValueKey"].Value;
                                                                        string matchValue = conditionNode.Attributes["ConditionMatchValue"].Value;
                                                                        condition = new HMNotifyCondition(devAddress, devValueKey, matchValue);
                                                                    }

                                                                    if(condition != null)
                                                                    {
                                                                        hmNotifyItem.AddCondition(condition);
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    errors.Add(ex);
                                                                }                                                                
                                                            }
                                                        }

                                                    }
                                                }

                                                // Add notify item to collection after setting the destination reference
                                                hmNotifyItem.SendNotificationTo = hmNotifyDest.Id;
                                                hmNotifyItems.Add(hmNotifyItem);
                                            }
                                            catch (Exception ex)
                                            {
                                                errors.Add(ex);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            errors.Add(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }

            return new Tuple<List<HMNotifyDestination>, List<HMNotifyItem>>(hmNotifyDestinations, hmNotifyItems);
        }

        /// <summary>
        /// Obtains an XML fragment from the configuration file representing a specific menu structure
        /// </summary>
        /// <param name="fileName">Path or name of the config file</param>
        /// <param name="configNodeElementName">Name of the config section</param>
        /// <returns>XML fragment as XmlNode object</returns>
        private XmlNode GetConfigXml(string fileName, string configNodeElementName)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(fileName) && !String.IsNullOrWhiteSpace(configNodeElementName))
                {
                    XmlNode configNode = null;
                    string configFilePath = String.Empty;
                    if (fileName.Contains(":\\"))
                    {
                        // try it as absolute path
                        configFilePath = fileName;
                    }
                    else
                    {
                        // try it as relative path
                        string fullPathToAssembly = Assembly.GetExecutingAssembly().Location;
                        configFilePath = fullPathToAssembly.Substring(0, fullPathToAssembly.LastIndexOf("\\")) + "\\" + fileName;
                    }

                    XmlReaderSettings readerSettings = new XmlReaderSettings();
                    readerSettings.IgnoreComments = true;
                    using (XmlReader reader = XmlReader.Create(configFilePath, readerSettings))
                    {
                        XmlDocument config = new XmlDocument();
                        config.Load(reader);
                        configNode = config.GetElementsByTagName(configNodeElementName)[0];
                    }
                    
                    if (configNode != null && configNode.ChildNodes.Count > 0)
                    {
                        return configNode;
                    }
                }
            }
            catch(Exception ex)
            {
                errors.Add(ex);
            }

            return null;
        }

        /// <summary>
        /// Get items currently written to local error log and deletes them
        /// </summary>
        /// <returns>Array of exceptions</returns>
        public Exception[] GetRecentErrors()
        {
            Exception[] recentErrors = errors.ToArray();
            errors.Clear();
            return recentErrors;
        }

#endregion

        /// <summary>
        /// Clean up
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (hmWrapper != null)
                {
                    // currently nothing to do due to change from XML-RPC to XML-API
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }
        }
    }
}
