# Reset Password activity

Adds a workflow activity that generates a reset password token for the specified user. You can define the source of the User object using a JavaScript expression. It will set the token and the URL to the workflow `LastResult` property and optionally it can set them to the `Properties` dictionary to a key that you define as an activity parameter.
