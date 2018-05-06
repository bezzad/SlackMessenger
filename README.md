# ![SlackMessenger Logo](https://a.slack-edge.com/9c217/img/loading_hash_animation.gif)  SlackMessenger

[![nuget version](https://img.shields.io/nuget/v/SlackMessenger.svg)](https://www.nuget.org/packages/SlackMessenger)
[![Nuget Downloads](https://img.shields.io/nuget/dt/SlackMessenger.svg)](https://www.nuget.org/packages/SlackMessenger)

A .NET library to enable you to send bot messages in slack, by posting messages to Slack's incoming WebHook Urls

## NuGet

Install via NuGet: ``` Install-Package SlackMessenger ```

[Or click here to go to the NuGet package landing page](https://www.nuget.org/packages/SlackMessenger)

## Registering for your WebHook Url with Slack

[Click here to set up an incoming WebHook](https://my.slack.com/services/new/incoming-webhook/)

- Sign in to Slack
- Choose a channel to post to
- Then click on the green button Add Incoming WebHooks integration
- You will be given a WebHook Url. Keep this private. Use it when you set up the SlackClient. See example below.

```C#
// Usage example
// This will send a message to the slack channel with the message, username and emoji of your choice 

using SlackMessenger;

var client = new SlackClient("https://hooks.slack.com/services/Your/WebHook/Url"); 

// send message by emoji icon
var msg = new Message($"Test Message for slack channel", "channelName", "username", ":emoji:"); 

// send message by icon from an url
var msg = new Message($"Test Message for slack channel", "channelName", "username", "http://test.com/icon.png"); 

client.Send(msg);

```
