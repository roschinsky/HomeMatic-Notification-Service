# HomeMatic-Notification-Service
A notification Windows Service for HomeMatic, using the HomeMatic-XmlApi-Lib which is based on XML-API Addon. The service is able to poll your Homematic CCU2 and detect status changes, like "a door was opened" or "a window was closed", that may lead to a notification sent to a push notification provider like Pushalot (Windows Phone or Windows 10) or Pushover (iOS and Android). 
There are also features included, to prevent from sending notification for single devices if conditions like time, variable- or device-status are detected. It is also possible to define multiple notification groups that will send to different provides and listen to different conditions or devices. All the configuration can be done by editing and XML file.

_(About the libraries and APIs: The XML-API Addon (http://www.homematic-inside.de/software/xml-api) is a free collection of scripts, packaged as installable addon for the 'HomeMatic' home automation system. The HomeMatic-XmlApi-Lib was built to use this Add-on in your .NET projects, so read more about it and get the latest version from https://github.com/roschinsky/HomeMatic-XmlApi-Lib.)_

## Contents

### SVC_HomeMaticNotification

The service itself. Using the *HMNotifier* you can connect to a HomeMatic CCU and trigger the test for status changes within the monitored devices by calling the _CollectEvents()_ method. 
The class *HMNotifyItem* is the object representation of an item from the configuration file. That means the definition of a device to monitor and all its properties. A *HMNotification* object is a captured event, that can generate a new push message, detected by the _CollectEvents()_ method.

![Class diagramm](https://troschinsky.files.wordpress.com/2015/12/homematicnotification_svc.png?w=600)

### TST_HomeMaticNotification

A very, very simple Windows Forms application that'll use the service and its classes to behave exactly like the service. The only difference is, that you can watch in real-time what the service will do, track down errors (e.g. in configuration) and see what devices will be monitored.

![Tester Windows Forms application](https://troschinsky.files.wordpress.com/2015/12/homematicnotification_tst.png)

If you just want to debug or test your configuration you can use the "Simulate sending" checkbox - the frontend will tell you about captured notification events but will not send it into the great wide open!

## Current Version

The current version of HomeMatic-Notification-Service is a beta release. Most of the functions will work as designed but there might be so issues that will occur in untested scenarios.
