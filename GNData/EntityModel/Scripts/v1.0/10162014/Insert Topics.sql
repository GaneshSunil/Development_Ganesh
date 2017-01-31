USE [gn_db]
GO

                      
          
DELETE FROM [gn].[GNNotificationSenders]
GO

INSERT INTO [gn].[GNNotificationSenders]
           ([Sender],[Name])
     VALUES
           ('noreply@genomenext.com','GenomeNext LLC <noreply@genomenext.com>')
		   GO


DELETE FROM [gn].[GNNotificationTopics]
GO
		   
INSERT INTO [gn].[GNNotificationTopics]
           ([Code],[Format],[Sender],[Priority],[Subject],[MessageTemplatePath],[Status],[CreatedBy],[CreateDateTime],[Title],[IsSubscriptionOptional],[Description],[NotifyObjectCreator],[Message])
     VALUES
           ('USER_ACCOUNT_SEND_INVITATION','EMAIL','telma.frege@genomenext.com','NORMAL','You are invited!',NULL,'ACTIVE','6927dfad-da8f-4436-af7f-ca18bcf98ba8'
           ,NULL ,'Invitations to join the Portal','N','Invitation to be sent to new users by an already existing user.','N'
           ,'<p><img alt="[GenomeNext LLC logo]" src="https://secure.genomenext.net/Images/genomenextllclogo.jpg" style="float:left; margin:10px" /></p>

<p>&nbsp;&nbsp;</p>

<div style="background:#00cc99;border:1px solid #00cc99;padding:5px 10px;"><span style="font-size:36px"><span style="font-family:verdana,geneva,sans-serif"><span style="color:#FFFFFF">You&#39;re Invited!</span></span></span></div>

<p>&nbsp;</p>

<p>Greetings, {FirstName},</p>

<p>You have been invited to be a user of the GenomeNext Analysis Portal by {OrganizationName}.</p>

<p>To accept this invitation and complete your user registration, please click on the following link:</p>

<div style="width: 400px; border: 1px solid rgb(204, 204, 204); padding: 5px 10px; text-align: center; background: rgb(238, 238, 238);"><a href="{InvitationUrl}"><strong>Accept My Invitation</strong></a></div>

<p>Thank You,<br />
The GenomeNext Team<br />
<a href="http://www.genomenext.com">www.genomenext.com</a></p>

<p><a href="http://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[Facebook]" src="https://secure.genomenext.net/Images/fb_icon.png" style="border:0px; float:left; height:22px; width:22px" /></a>&nbsp;&nbsp;&nbsp;<a href="https://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[Twitter]" src="https://secure.genomenext.net/Images/tw_icon.png" style="float:left; height:22px; width:22px" /></a>&nbsp;&nbsp;&nbsp;<a href="https://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[LinkedIn]" src="https://secure.genomenext.net/Images/in_icon.png" style="float:left; height:22px; width:22px" /></a></p>

<p><span style="color:#A9A9A9"><span style="font-size:10px">This message was sent by an automated notification server. Please do not reply to this email. If you would like to contact a member of our team, please <u>email &lt;insert email&gt;</u> or call us at 888-555-3333.&nbsp;</span></span><span style="color:#A9A9A9"><span style="font-size:10px">To change your notification preferences, <u>click here.</u></span></span></p>
')
GO

INSERT INTO [gn].[GNNotificationTopics]
           ([Code],[Format],[Sender],[Priority],[Subject],[MessageTemplatePath],[Status],[CreatedBy],[CreateDateTime],[Title],[IsSubscriptionOptional],[Description],[NotifyObjectCreator],[Message])
     VALUES
           ('USER_ACCOUNT_RESET_PASSWORD','EMAIL','telma.frege@genomenext.com','NORMAL','Reset your password.',NULL,'ACTIVE','6927dfad-da8f-4436-af7f-ca18bcf98ba8'
           ,NULL ,'Reset Password','N','E-mail to send the users a link to reset their passwords.','N'
           ,'<p><img alt="[GenomeNext LLC logo]" src="https://secure.genomenext.net/Images/genomenextllclogo.jpg" style="float:left; margin:10px" /></p>

<p>&nbsp;&nbsp;</p>

<div style="background:#00cc99;border:1px solid #00cc99;padding:5px 10px;"><span style="color:#FFFFFF"><span style="font-size:36px">Password Reset</span></span></div>

<p>&nbsp;</p>

<p>Please click the following link to reset your password:</p>

<div style="width: 400px; border: 1px solid rgb(204, 204, 204); padding: 5px 10px; text-align: center; background: rgb(238, 238, 238);"><a href="{ResetPasswordUrl}"><strong>Reset My Password</strong></a></div>

<p>Thank You,<br />
The GenomeNext Team<br />
<a href="http://www.genomenext.com">www.genomenext.com</a></p>

<p><a href="http://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[Facebook]" src="https://secure.genomenext.net/Images/fb_icon.png" style="border:0px; float:left; height:22px; width:22px" /></a>&nbsp;&nbsp;&nbsp;<a href="https://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[Twitter]" src="https://secure.genomenext.net/Images/tw_icon.png" style="float:left; height:22px; width:22px" /></a>&nbsp;&nbsp;&nbsp;<a href="https://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[LinkedIn]" src="https://secure.genomenext.net/Images/in_icon.png" style="float:left; height:22px; width:22px" /></a></p>

<p><span style="color:#A9A9A9"><span style="font-size:10px">This message was sent by an automated notification server. Please do not reply to this email. If you would like to contact a member of our team, please <u>email &lt;insert email&gt;</u> or call us at 888-555-3333.&nbsp;</span></span><span style="color:#A9A9A9"><span style="font-size:10px">To change your notification preferences, <u>click here.</u></span></span></p>
')
GO
           
INSERT INTO [gn].[GNNotificationTopics]
           ([Code],[Format],[Sender],[Priority],[Subject],[MessageTemplatePath],[Status],[CreatedBy],[CreateDateTime],[Title],[IsSubscriptionOptional],[Description],[NotifyObjectCreator],[Message])
     VALUES
           ('USER_ACCOUNT_CONFIRM_EMAIL','EMAIL','telma.frege@genomenext.com','NORMAL','Welcome! New user account created.',NULL,'ACTIVE','6927dfad-da8f-4436-af7f-ca18bcf98ba8'
           ,NULL ,'Confirm e-mail account','N','E-mail sent to the user once an account is created, requesting her/him to confirm the address entered.','N'
           ,'<p><img alt="[GenomeNext LLC logo]" src="https://secure.genomenext.net/Images/genomenextllclogo.jpg" style="float:left; margin:10px" /></p>

<p>&nbsp;&nbsp;</p>

<div style="background:#00cc99;border:1px solid #00cc99;padding:5px 10px;"><span style="font-size:36px"><span style="font-family:verdana,geneva,sans-serif"><span style="color:#FFFFFF">Welcome to GenomeNext!</span></span></span></div>

<p><span style="font-size:22px"><strong>A new user account has been created.</strong></span></p>

<div>
<p>Hello, {First Name}</p>

<p>Thank you for taking the time to create an account with GenomeNext. To complete your user profile and change your password, click on the button below.</p>
</div>

<div style="width: 400px; border: 1px solid rgb(204, 204, 204); padding: 5px 10px; text-align: center; background: rgb(238, 238, 238);"><a href="{RegistrationUrl}"><strong>Click Here to Complete Your Registration</strong></a></div>

<p>Now that you have created your account, you will be able to take advantage of our portal features, that include adding users to your organization, uploading samples, creating analyses, and managing your billing options.</p>

<div style="line-height: 20.7999992370605px;">
<p>We look forward to providing you with the fastest, most cost effective and accurate bioinformatics tools for your clinical and research needs.</p>

<div>Thank You,<br />
The GenomeNext Team<br />
<a href="http://www.genomenext.com">www.genomenext.com</a></div>
</div>

<p><a href="http://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[Facebook]" src="https://secure.genomenext.net/Images/fb_icon.png" style="border:0px; float:left; height:22px; width:22px" /></a>&nbsp;&nbsp;&nbsp;<a href="https://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[Twitter]" src="https://secure.genomenext.net/Images/tw_icon.png" style="float:left; height:22px; width:22px" /></a>&nbsp;&nbsp;&nbsp;<a href="https://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[LinkedIn]" src="https://secure.genomenext.net/Images/in_icon.png" style="float:left; height:22px; width:22px" /></a></p>

<p><span style="color:#A9A9A9"><span style="font-size:10px">This message was sent by an automated notification server. Please do not reply to this email. If you would like to contact a member of our team, please <u>email &lt;insert email&gt;</u> or call us at 888-555-3333.&nbsp;</span></span><span style="color:#A9A9A9"><span style="font-size:10px">To change your notification preferences, <u>click here.</u></span></span></p>
')
GO
           
INSERT INTO [gn].[GNNotificationTopics]
           ([Code],[Format],[Sender],[Priority],[Subject],[MessageTemplatePath],[Status],[CreatedBy],[CreateDateTime],[Title],[IsSubscriptionOptional],[Description],[NotifyObjectCreator],[Message])
     VALUES
           ('USER_ACCOUNT_CHANGE_PASSWORD','EMAIL','telma.frege@genomenext.com','NORMAL','Password changed.',NULL,'ACTIVE','6927dfad-da8f-4436-af7f-ca18bcf98ba8'
           ,NULL ,'Change Password','N','E-mail sent after the user changes his/her password.','N'
           ,'<p><img alt="[GenomeNext LLC logo]" src="https://secure.genomenext.net/Images/genomenextllclogo.jpg" style="float:left; margin:10px" /></p>

<p>&nbsp;&nbsp;</p>

<div style="background:#00cc99;border:1px solid #00cc99;padding:5px 10px;"><span style="color:#FFFFFF"><span style="font-size:36px">Password Changed</span></span></div>

<p><span style="font-size:22px"><strong>GenomeNext Account Password Changed.</strong></span></p>

<div>
<p>You are receiving this email because you have updated your&nbsp;account password through our online portal.</p>

<p>If you have not performed this update or believe it is a mistake, please contact us directly at 888-555-3333.</p>
</div>

<p>Thank You,<br />
The GenomeNext Team<br />
<a href="http://www.genomenext.com">www.genomenext.com</a></p>

<p><a href="http://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[Facebook]" src="https://secure.genomenext.net/Images/fb_icon.png" style="border:0px; float:left; height:22px; width:22px" /></a>&nbsp;&nbsp;&nbsp;<a href="https://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[Twitter]" src="https://secure.genomenext.net/Images/tw_icon.png" style="float:left; height:22px; width:22px" /></a>&nbsp;&nbsp;&nbsp;<a href="https://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[LinkedIn]" src="https://secure.genomenext.net/Images/in_icon.png" style="float:left; height:22px; width:22px" /></a></p>

<p><span style="color:#A9A9A9"><span style="font-size:10px">This message was sent by an automated notification server. Please do not reply to this email. If you would like to contact a member of our team, please <u>email &lt;insert email&gt;</u> or call us at 888-555-3333.&nbsp;</span></span><span style="color:#A9A9A9"><span style="font-size:10px">To change your notification preferences, <u>click here.</u></span></span></p>
')
GO
           
INSERT INTO [gn].[GNNotificationTopics]
           ([Code],[Format],[Sender],[Priority],[Subject],[MessageTemplatePath],[Status],[CreatedBy],[CreateDateTime],[Title],[IsSubscriptionOptional],[Description],[NotifyObjectCreator],[Message])
     VALUES
           ('ORG_ACCOUNT_REGISTRATION_COMPLETE','EMAIL','telma.frege@genomenext.com','NORMAL','Registration Completed. Welcome to GenomeNext!',NULL,'ACTIVE','6927dfad-da8f-4436-af7f-ca18bcf98ba8'
           ,NULL ,'Organization Registration Completed','N','E-mail notifying the organitation main contact that the registration is completed.','N'
           ,'<p><img alt="[GenomeNext LLC logo]" src="https://secure.genomenext.net/Images/genomenextllclogo.jpg" style="float:left; margin:10px" /></p>

<p>&nbsp;&nbsp;</p>

<div style="background:#00cc99;border:1px solid #00cc99;padding:5px 10px;"><span style="font-size:36px"><span style="font-family:verdana,geneva,sans-serif"><span style="color:#FFFFFF">Welcome to GenomeNext!</span></span></span></div>

<p><span style="font-size:22px"><strong>The Registration Process is completed.</strong></span></p>

<div>
<p>Hello, {First Name}</p>

<p>Thank you for taking the time to create an account with GenomeNext.</p>
</div>

<p>Now that you have created your account, you will be able to take advantage of our portal features, that include adding users to your organization, uploading samples, creating analyses, and managing your billing options.</p>

<div style="line-height: 20.7999992370605px;">
<p>We look forward to providing you with the fastest, most cost effective and accurate bioinformatics tools for your clinical and research needs.</p>

<div>Thank You,<br />
The GenomeNext Team<br />
<a href="http://www.genomenext.com">www.genomenext.com</a></div>
</div>

<p><a href="http://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[Facebook]" src="https://secure.genomenext.net/Images/fb_icon.png" style="border:0px; float:left; height:22px; width:22px" /></a>&nbsp;&nbsp;&nbsp;<a href="https://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[Twitter]" src="https://secure.genomenext.net/Images/tw_icon.png" style="float:left; height:22px; width:22px" /></a>&nbsp;&nbsp;&nbsp;<a href="https://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[LinkedIn]" src="https://secure.genomenext.net/Images/in_icon.png" style="float:left; height:22px; width:22px" /></a></p>

<p><span style="color:#A9A9A9"><span style="font-size:10px">This message was sent by an automated notification server. Please do not reply to this email. If you would like to contact a member of our team, please <u>email &lt;insert email&gt;</u> or call us at 888-555-3333.&nbsp;</span></span><span style="color:#A9A9A9"><span style="font-size:10px">To change your notification preferences, <u>click here.</u></span></span></p>
')
GO
           

