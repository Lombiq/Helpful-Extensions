# Emails and Email Templates

## Email Templates

Provides a shape-based email template rendering service. The email templates are represented by email template IDs that are also used to identify the corresponding shape using the following pattern: `EmailTemplate__{EmailTemplateID}`. E.g., for the `ContactUs` email template you need to create a shape with the `EmailTemplate__ContactUs` shape type.

In the email template shapes use the `Layout__EmailTemplate` as the `ViewLayout` to wrap it with a simple HTML layout.

To extend the layout you can override the `EmailTemplate_LayoutInjections` shape and inject content to the specific zones provided by the layout to activate it in every email template. E.g.,

```html
<zone name="Footer">
    Best,<br>
    My Awesome Team
</zone>
```

To add inline styles include:

```html
<zone name="Head">
    <style>
        /* CSS code... */
    </style>
</zone>
```

## Deferred email sending

Use the `ShellScope.Current.SendEmailDeferred()` for sending emails. It'll send emails after the shell scope has ended without blocking the request.