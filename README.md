# MailingFerret
A simple Email Sender with Razor email templates support for .NET Core.

It requires access to a SMTP. Razor email templates are optional.

# How to install
You can install this package via Nuget (https://www.nuget.org/packages/MailingFerret/). 

# Setting up
First, register *MailingFerret* as a Service in *Startup.cs#:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddMailingFerret();
    ...
}
```

This will add an *IEmailSender* and an *IViewRenderService* implementations as services to the DI container. The *IEmailSender* service is used to send emails, while the *IViewRenderService* renders Razor email templates to the email body.

Note: you can provide your own implementation of *IViewRenderService* if you want. Don't forget to add it as a service before adding *MailingFerret*.

Next step is to provide an SMTP to work with. You need to provide the following information in the *appsettins.json* file of your project:

```json
...
  "MailingFerret":
  {
    "EmailHost": "your.emailhost.com",
    "EmailUser": "your.user",
    "EmailPassword": "your.password",
    "EmailAccount": "your@account.com"
  }
...
```

That's all. You are ready to send emails using razor templates, as long as they are stored next to your Views (i.e: inside  a folder like *Views/EmailTemplates/* or *Views/Shared/EmailTemplates/*).

# How to use it

To send an email, you can use any overload of the *SendEmailAsync* method of *IEmailSender*. The example below shows how to send an email using a model and a razor template, assuming that this template is inside a folder named *EmailTemplates*:
```csharp
//In this example _emailSender implements IEmailSender.
EmailModel emailModel = new EmailModel();
_emailSender.SendEmailAsync("some@email.com", "subject", "EmailTemplates/EmailModelTemplate", emailModel);
```
