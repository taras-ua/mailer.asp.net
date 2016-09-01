# mailer.asp.net
An example of how you can organise HTML emails sending from your ASP.NET MVC project.

## Long story short
To send HTML emails from your app copy contents of the `dist` folder into your project folder, configure everything inside those files you copied and use

```csharp
AspNetMailer.EmailController.Send(HttpContext.Request.RequestContext, "recipient@example.com", "Subject", "ViewName", viewModel);
```

## Long story long

To send HTML emails from your app follow this steps:

##### Step 1

Clone this repository:

```bash
$ git clone https://github.com/taras-ua/mailer.asp.net.git ./mailer
```

##### Step 2

Copy contents of the `dist` folder into your project folder (to the same level as your `Web.config` file). You will be asked to merge folders you are copying with existing folders of your project and should accept.

##### Step 3

Add your SMTP configurations into `<appSettings>` section of your `Web.config` as following:
 
 ```xml
<appSettings>
    <!-- ... your other settings ... -->
    <add key="AuthEmail" value="sender@your.domain" />
    <add key="AuthPassword" value="Your!Passw0rd" />
    <add key="AuthHost" value="smtp.your.domain" />
    <add key="AuthPort" value="465" />
</appSettings>
```  

##### Step 4 (optional)

Change default email layouts, add new email views and configure everything to suit your needs.

The minimum configuration you would probably want to do:

1. Set path to your logo at `Views\Shared\_EmailLayout.cshtml` line 10.
2. Set the name of your company at `Views\Shared\_EmailLayout.cshtml` line 11.
3. Set the address of your company at `Views\Shared\_EmailLayout.cshtml` line 12.
4. Set your social accounts links at `Views\Shared\_EmailLayout.cshtml` line 13 and 14.
5. Change emails font at `Views\Shared\_EmailLayout.cshtml` line 16.
6. Provide your custom fonts at `Views\Shared\_EmailLayout.cshtml` line 20.

You can also make further customisation of `Views\Shared\_EmailLayout.cshtml` master layout and email layouts in `Views\Email`. Have in mind that a lot of mail clients don't tolerate `<style>` tags, so **you should use `style=""` attributes**  for better compatibility.

##### Step 5

Send HTML emails from your controllers with `AspNetMailer.EmailController.Send()` method.

There are 2 email layouts in example:

1. `Confirmation` layout for "Confirm your email address" letters.
2. `News` layout for news update emails.

To create new views simply add a `YourNewView.cshtml` file to the `Views\Email` folder and a `YourNewEmailViewModel : EmailViewModelBase` class to the `Models\EmailViewModels.cs` file.

Here is an example of `AspNetMailer.EmailController.Send()` usage for account confirmation letter:

```csharp
// Register a confirmation code
string code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
// Build your confirmation url
string confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code })
// Create a model to render email with
var model = new Models.ConfirmationEmailViewModel
{
    ConfirmationLink = Request.Url.Scheme + "://" + Request.Url.Authority + confirmationLink
};
// Add a link to the web version of your rendered email (optional)
model.WebVersion = Request.Url.Scheme + "://" + Request.Url.Authority
    + Url.Action(AspNetMailer.EmailController.CONFIRMATION_EMAIL, "Email", model);
// Send your email
AspNetMailer.EmailController.Send(HttpContext.Request.RequestContext, user.Email,
    "Confirm your account", AspNetMailer.EmailController.CONFIRMATION_EMAIL, model);
```

## Contribution

Please, **do** contribute to this repository.

Current TODOs are:

* Improve `Views\Shared\_EmailLayout.cshtml` layout (remove unnecessary styles and prettify layout).
* Make some script to automate installation steps 1 to 4.
* Come up with more TODOs.

The `Mailer` example project includes some forms to test default emails. Simply add your SMTP configurations to `Web.config` and send test emails from the web app's homepage.

To add your contributions to `dist` folder, use a gulp script from root folder like this:

```bash
$ npm install
$ gulp build
```
