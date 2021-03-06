USE [gn_db]
GO
SET ANSI_PADDING OFF
GO

INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'49af631a-9d02-49d1-a683-49063a1cda03', N'GN_IT_ARCHITECTURE')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'4aa47339-9413-423a-8304-940308d91e36', N'TEAM_MANAGER')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'4b8cf395-eac0-4b4e-956e-9fc84277c35b', N'GN_IT_GENOMICS')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'5d14cef9-cf3e-45e2-8492-943ad599d9e0', N'GN_IT_WEB')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'TEAM_MEMBER')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'7a43c68d-4021-4f92-abfb-8a671508ea08', N'GN_FINANCE')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'7c797503-7367-4bd5-ab0e-bd759ddd2142', N'ORG_MANAGER')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'88ac5ddb-82f6-45f6-b86a-401573b7eac1', N'GN_SUPPORT')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'8e73e782-0855-4785-9926-16f5a3829b8f', N'PROJECT_MANAGER')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'eb3e3558-9068-40c6-809b-8012c29a5d37', N'GN_ADMIN')

INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [DefaultOrganizationId]) VALUES (N'79e69edc-cfff-46b8-9e8a-bbc3783d70d1', N'elio.navarro@genomenext.com', 1, N'ALsZdWlMngKiykY6ih/mCG2h9XwUZbp11LR8RmO88KzIHarUSbajwaFQJA6HpsDfSw==', N'ee3958ff-c106-4430-84e6-e5a09d4ed49b', NULL, 0, 0, NULL, 0, 0, N'elio.navarro@genomenext.com', N'82ff6451-dac6-4a88-a967-cdaf5f9a3599')

INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'79e69edc-cfff-46b8-9e8a-bbc3783d70d1', N'eb3e3558-9068-40c6-809b-8012c29a5d37')

INSERT [gn].[AWSRegions] ([AWSRegionSystemName], [CreatedBy], [CreateDateTime]) VALUES (N'us-east-1', NULL, NULL)

INSERT [gn].[AWSConfigs] ([Id], [AWSAccessKeyId], [AWSSecretAccessKey], [AWSRegionSystemName], [CreatedBy], [CreateDateTime]) VALUES (N'd740808b-a180-4e4c-be08-0a474fb0912a', N'AKIAIG24CVVPPXRBQQDQ', N'Ux4/YhN/l6085AbsBZnWJHKczpINRQEDtJCLeDx5', N'us-east-1', NULL, NULL)

SET IDENTITY_INSERT [gn].[AWSResourceTypes] ON 

INSERT [gn].[AWSResourceTypes] ([Id], [Name], [CreatedBy], [CreateDateTime]) VALUES (1, N'S3 Bucket', NULL, NULL)
INSERT [gn].[AWSResourceTypes] ([Id], [Name], [CreatedBy], [CreateDateTime]) VALUES (2, N'SQS Queue', NULL, NULL)
INSERT [gn].[AWSResourceTypes] ([Id], [Name], [CreatedBy], [CreateDateTime]) VALUES (3, N'SNS Topic', NULL, NULL)
INSERT [gn].[AWSResourceTypes] ([Id], [Name], [CreatedBy], [CreateDateTime]) VALUES (4, N'EC2 Instance', NULL, NULL)
INSERT [gn].[AWSResourceTypes] ([Id], [Name], [CreatedBy], [CreateDateTime]) VALUES (5, N'AMI', NULL, NULL)
SET IDENTITY_INSERT [gn].[AWSResourceTypes] OFF

INSERT [gn].[AWSResources] ([Id], [ARN], [AWSConfigId], [AWSResourceTypeId], [CreatedBy], [CreateDateTime]) VALUES (N'60f93af7-2e32-4b38-8481-13e5d57e1070', N'dev-gn-s3-01', N'd740808b-a180-4e4c-be08-0a474fb0912a', 1, N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-09-29 22:36:37.007' AS DateTime))
INSERT [gn].[AWSResources] ([Id], [ARN], [AWSConfigId], [AWSResourceTypeId], [CreatedBy], [CreateDateTime]) VALUES (N'32c529e4-e033-4bbd-b867-141429e1bb0b', N'GN_ANALYSIS_RESULT', N'd740808b-a180-4e4c-be08-0a474fb0912a', 2, NULL, NULL)
INSERT [gn].[AWSResources] ([Id], [ARN], [AWSConfigId], [AWSResourceTypeId], [CreatedBy], [CreateDateTime]) VALUES (N'c353588c-002e-47ae-bda9-2315f442323c', N'GN_ANALYSIS_STATUS', N'd740808b-a180-4e4c-be08-0a474fb0912a', 2, NULL, NULL)
INSERT [gn].[AWSResources] ([Id], [ARN], [AWSConfigId], [AWSResourceTypeId], [CreatedBy], [CreateDateTime]) VALUES (N'7021cf4c-99ff-4508-a827-5e9e8708eaa2', N'dev-gn-s3-02', N'd740808b-a180-4e4c-be08-0a474fb0912a', 1, N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-09-29 22:36:55.700' AS DateTime))
INSERT [gn].[AWSResources] ([Id], [ARN], [AWSConfigId], [AWSResourceTypeId], [CreatedBy], [CreateDateTime]) VALUES (N'f5ce822e-c7e2-45ec-a4b0-86b1a1fd2061', N'ami-4afd7522', N'd740808b-a180-4e4c-be08-0a474fb0912a', 5, NULL, NULL)
INSERT [gn].[AWSResources] ([Id], [ARN], [AWSConfigId], [AWSResourceTypeId], [CreatedBy], [CreateDateTime]) VALUES (N'7237ba29-caaf-4ac2-a4f0-d13f5f56d3f8', N'GN_ANALYSIS_REQUEST', N'd740808b-a180-4e4c-be08-0a474fb0912a', 2, NULL, NULL)

SET IDENTITY_INSERT [gn].[GNAccountTypes] ON 

INSERT [gn].[GNAccountTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (1, N'INDUSTRY', N'Industry', NULL, NULL)
INSERT [gn].[GNAccountTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (2, N'ACADEMIC', N'Academic / Non-Profit', NULL, NULL)
SET IDENTITY_INSERT [gn].[GNAccountTypes] OFF
SET IDENTITY_INSERT [gn].[GNAnalysisSampleAffectedIndicators] ON 

INSERT [gn].[GNAnalysisSampleAffectedIndicators] ([Id], [Code], [Name], [CreatedBy], [CreateDateTime]) VALUES (1, N'Y', N'YES                                               ', N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-10-07 22:02:49.793' AS DateTime))
INSERT [gn].[GNAnalysisSampleAffectedIndicators] ([Id], [Code], [Name], [CreatedBy], [CreateDateTime]) VALUES (2, N'N', N'NO                                                ', N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-10-07 22:02:55.277' AS DateTime))
INSERT [gn].[GNAnalysisSampleAffectedIndicators] ([Id], [Code], [Name], [CreatedBy], [CreateDateTime]) VALUES (3, N'U', N'UNKNOWN                                           ', N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-10-07 22:03:02.607' AS DateTime))
SET IDENTITY_INSERT [gn].[GNAnalysisSampleAffectedIndicators] OFF
SET IDENTITY_INSERT [gn].[GNAnalysisTypes] ON 

INSERT [gn].[GNAnalysisTypes] ([Id], [Name], [TypeValue], [CreatedBy], [CreateDateTime]) VALUES (1, N'Whole Genome', N'GENOME', NULL, NULL)
INSERT [gn].[GNAnalysisTypes] ([Id], [Name], [TypeValue], [CreatedBy], [CreateDateTime]) VALUES (2, N'Exome', N'EXOME', NULL, NULL)
INSERT [gn].[GNAnalysisTypes] ([Id], [Name], [TypeValue], [CreatedBy], [CreateDateTime]) VALUES (3, N'Targ. Panel', N'EXOME', NULL, NULL)
SET IDENTITY_INSERT [gn].[GNAnalysisTypes] OFF
SET IDENTITY_INSERT [gn].[GNCloudFileCategories] ON 

INSERT [gn].[GNCloudFileCategories] ([Id], [Name], [FolderPath], [CreatedBy], [CreateDateTime], [Type], [FileExtensions]) VALUES (1, N'FASTQ File', N'fastq', NULL, NULL, N'INPUT', N'.fastq, .fastq.gz, .fastq.zip, .fastq.rar')
INSERT [gn].[GNCloudFileCategories] ([Id], [Name], [FolderPath], [CreatedBy], [CreateDateTime], [Type], [FileExtensions]) VALUES (2, N'Variant Call File', N'results', NULL, NULL, N'OUTPUT', N'.vcf')
INSERT [gn].[GNCloudFileCategories] ([Id], [Name], [FolderPath], [CreatedBy], [CreateDateTime], [Type], [FileExtensions]) VALUES (3, N'Annotation', N'results', NULL, NULL, N'OUTPUT', N'.table, .xls, .xlsx')
INSERT [gn].[GNCloudFileCategories] ([Id], [Name], [FolderPath], [CreatedBy], [CreateDateTime], [Type], [FileExtensions]) VALUES (5, N'BAM file', N'results', NULL, NULL, N'OUTPUT', N'.bam')
INSERT [gn].[GNCloudFileCategories] ([Id], [Name], [FolderPath], [CreatedBy], [CreateDateTime], [Type], [FileExtensions]) VALUES (6, N'Results file', N'results', NULL, NULL, N'OUTPUT', N'.')
SET IDENTITY_INSERT [gn].[GNCloudFileCategories] OFF

INSERT [gn].[GNOrganizations] ([Id], [Name], [GNContactId], [AWSConfigId], [UTCOffset], [CreatedBy], [CreateDateTime], [UTCOffsetDescription]) VALUES (N'82ff6451-dac6-4a88-a967-cdaf5f9a3599', N'GenomeNext, LLC', NULL, N'd740808b-a180-4e4c-be08-0a474fb0912a', N'-05:00', NULL, NULL, N'(UTC-05:00) Eastern Time (US & Canada)')

INSERT [gn].[GNContacts] ([Id], [FirstName], [LastName], [Title], [Phone], [Fax], [Email], [Website], [StreetAddress1], [StreetAddress2], [City], [State], [Zip], [IsInviteAccepted], [AspNetUserId], [GNOrganizationId], [CreatedBy], [CreateDateTime], [TermsAcceptDateTime], [PrivacyPolicyAcceptDateTime]) VALUES (N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'Elio', N'Navarro', NULL, NULL, NULL, N'elio.navarro@genomenext.com', NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'79e69edc-cfff-46b8-9e8a-bbc3783d70d1', N'82ff6451-dac6-4a88-a967-cdaf5f9a3599', NULL, NULL, CAST(N'2014-09-30 01:53:00.923' AS DateTime), CAST(N'2014-09-30 01:53:00.923' AS DateTime))

UPDATE [gn].[GNOrganizations] 
SET [GNContactId] = N'4d488c52-5dec-4d16-8d53-98684b2901c8' 
WHERE [Id] = N'82ff6451-dac6-4a88-a967-cdaf5f9a3599'

INSERT [gn].[GNContactRoles] ([GNContactId], [AspNetRoleId], [CreatedBy], [CreateDateTime]) VALUES (N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'4aa47339-9413-423a-8304-940308d91e36', N'11a16e2a-d759-4d78-bc51-62af988e819a', CAST(N'2014-09-29 22:43:20.940' AS DateTime))
INSERT [gn].[GNContactRoles] ([GNContactId], [AspNetRoleId], [CreatedBy], [CreateDateTime]) VALUES (N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'11a16e2a-d759-4d78-bc51-62af988e819a', CAST(N'2014-09-29 22:43:20.940' AS DateTime))
INSERT [gn].[GNContactRoles] ([GNContactId], [AspNetRoleId], [CreatedBy], [CreateDateTime]) VALUES (N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'7c797503-7367-4bd5-ab0e-bd759ddd2142', N'11a16e2a-d759-4d78-bc51-62af988e819a', CAST(N'2014-09-29 22:43:20.940' AS DateTime))
INSERT [gn].[GNContactRoles] ([GNContactId], [AspNetRoleId], [CreatedBy], [CreateDateTime]) VALUES (N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'8e73e782-0855-4785-9926-16f5a3829b8f', N'11a16e2a-d759-4d78-bc51-62af988e819a', CAST(N'2014-09-29 22:43:20.940' AS DateTime))


INSERT [gn].[GNNotificationSenders] ([Sender], [Name]) VALUES (N'billing@genomenext.com                            ', N'billing@genomenext.com                                                                              ')
INSERT [gn].[GNNotificationSenders] ([Sender], [Name]) VALUES (N'feedback@genomenext.com                           ', N'feedback@genomenext.com                                                                             ')
INSERT [gn].[GNNotificationSenders] ([Sender], [Name]) VALUES (N'info@genomenext.com                               ', N'info@genomenext.com                                                                                 ')
INSERT [gn].[GNNotificationSenders] ([Sender], [Name]) VALUES (N'noreply@genomenext.com                            ', N'GenomeNext LLC <noreply@genomenext.com>                                                             ')
INSERT [gn].[GNNotificationSenders] ([Sender], [Name]) VALUES (N'privacy@genomenext.com                            ', N'privacy@genomenext.com                                                                              ')
INSERT [gn].[GNNotificationSenders] ([Sender], [Name]) VALUES (N'security@genomenext.com                           ', N'security@genomenext.com                                                                             ')
INSERT [gn].[GNNotificationSenders] ([Sender], [Name]) VALUES (N'support@genomenext.com                            ', N'support@genomenext.com                                                                              ')
INSERT [gn].[GNNotificationSenders] ([Sender], [Name]) VALUES (N'elio.navarro@genomenext.com                        ', N'elio.navarro@genomenext.com                                                         ')
SET IDENTITY_INSERT [gn].[GNNotificationTopics] ON 

INSERT [gn].[GNNotificationTopics] ([Id], [Code], [Format], [Sender], [Priority], [Subject], [Message], [MessageTemplatePath], [Status], [CreatedBy], [CreateDateTime], [Title], [IsSubscriptionOptional], [Description], [NotifyObjectCreator]) VALUES (2, N'USER_ACCOUNT_SEND_INVITATION', N'EMAIL', N'noreply@genomenext.com                            ', N'NORMAL', N'You are invited!', N'<p><img alt="[GenomeNext LLC logo]" src="https://secure.genomenext.net/Images/genomenextllclogo.jpg" style="float:left; margin:10px" /></p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<div style="background:#00cc99;border:1px solid #00cc99;padding:5px 10px;"><span style="font-size:36px"><span style="font-family:verdana,geneva,sans-serif"><span style="color:#FFFFFF">You&#39;re Invited!</span></span></span></div>

<p>Hello&nbsp;{FirstName},</p>

<p>You have been invited to be a user of the GenomeNext Analysis Portal by {InvitedByName} from {OrganizationName}.</p>

<p>To accept this invitation and complete your user registration, please click on the following link:</p>

<div style="width: 400px; border: 1px solid rgb(204, 204, 204); padding: 5px 10px; text-align: center; background: rgb(238, 238, 238);"><a href="{InvitationUrl}"><strong>Accept My Invitation</strong></a></div>

<p>Thank You,<br />
<br />
The GenomeNext Team<br />
<a href="http://www.genomenext.com">www.genomenext.com</a></p>

<table align="left" border="0" cellpadding="1" cellspacing="1" style="width:90px">
	<tbody>
		<tr>
			<td><a href="http://www.facebook.com/genomenext" style="text-decoration: none;"><img alt="[Facebook]" src="https://secure.genomenext.net/Images/fb_icon.png" style="border-width:0px; float:left; height:22px; width:22px" /></a></td>
			<td><a href="http://twitter.com/genomenext"><img alt="[Twitter]" src="https://secure.genomenext.net/Images/tw_icon.png" style="float:left; height:22px; width:22px" /></a></td>
			<td><a href="https://www.linkedin.com/company/3682901?trk=tyah&amp;trkInfo=idx%3A1-1-1%2CtarId%3A1415322712949%2Ctas%3Agenomenext"><img alt="[LinkedIn]" src="https://secure.genomenext.net/Images/in_icon.png" style="float:left; height:22px; width:22px" /></a></td>
		</tr>
	</tbody>
</table>

<p>&nbsp;&nbsp;&nbsp;<br />
&nbsp;</p>

<p><span style="color:#A9A9A9"><span style="font-size:10px">This message was sent by an automated notification server. Please do not reply to this email. If you would like to contact a member of our team, please <a href="mailto:support@genomenext.com">e-mail us</a>.&nbsp;This message and its contents are part of GenomeNext, LLC <a href="https://secure.genomenext.net/About/PrivacyPolicy">Privacy Policy</a> and <a href="https://secure.genomenext.net/About/TermsOfService">Terms of Service</a>. </span></span></p>

<p><object id="SILOBFWOBJECTID" style="width: 0px; height: 0px; display: block;"></object></p>
', NULL, N'ACTIVE', N'819af873-10bf-41c1-8a98-17b1402dc242', CAST(N'2014-11-07 01:16:40.937' AS DateTime), N'Invitations to join the Portal                    ', N'N', N'Invitation to be sent to new users by an already existing user.', N'Y')
INSERT [gn].[GNNotificationTopics] ([Id], [Code], [Format], [Sender], [Priority], [Subject], [Message], [MessageTemplatePath], [Status], [CreatedBy], [CreateDateTime], [Title], [IsSubscriptionOptional], [Description], [NotifyObjectCreator]) VALUES (3, N'USER_ACCOUNT_RESET_PASSWORD', N'EMAIL', N'noreply@genomenext.com                            ', N'NORMAL', N'Reset your password.', N'<p><img alt="[GenomeNext LLC logo]" src="https://secure.genomenext.net/Images/genomenextllclogo.jpg" style="float:left; margin:10px" /></p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<div style="background:#00cc99;border:1px solid #00cc99;padding:5px 10px;"><span style="color:#FFFFFF"><span style="font-size:36px">Password Reset</span></span></div>

<p>&nbsp;</p>

<p>Hello {FirstName},</p>

<p>You have submitted a request to reset your password.&nbsp;Please click the following link to reset your password:&nbsp;</p>

<div style="width: 400px; border: 1px solid rgb(204, 204, 204); padding: 5px 10px; text-align: center; background: rgb(238, 238, 238);"><a href="{ResetPasswordUrl}"><strong>Reset My Password</strong></a></div>

<p>If you have any questions, please contact GenomeNext customer <a href="mailto:support@genomenext.com?subject=Password%20Reset%20Support">support.</a></p>

<p>Thank You,<br />
<br />
The&nbsp;GenomeNext&nbsp;Team<br />
<a href="http://www.genomenext.com">www.genomenext.com</a></p>

<table align="left" border="0" cellpadding="1" cellspacing="1" style="width:90px">
	<tbody>
		<tr>
			<td><a href="http://www.facebook.com/genomenext" style="text-decoration: none;"><img alt="[Facebook]" src="https://secure.genomenext.net/Images/fb_icon.png" style="border-width:0px; float:left; height:22px; width:22px" /></a></td>
			<td><a href="http://twitter.com/genomenext"><img alt="[Twitter]" src="https://secure.genomenext.net/Images/tw_icon.png" style="float:left; height:22px; width:22px" /></a></td>
			<td><a href="https://www.linkedin.com/company/3682901?trk=tyah&amp;trkInfo=idx%3A1-1-1%2CtarId%3A1415322712949%2Ctas%3Agenomenext"><img alt="[LinkedIn]" src="https://secure.genomenext.net/Images/in_icon.png" style="float:left; height:22px; width:22px" /></a></td>
		</tr>
	</tbody>
</table>

<p>&nbsp;&nbsp;&nbsp;<br />
&nbsp;</p>

<p><span style="color:rgb(169, 169, 169)"><span style="font-size:10px">This message was sent by an automated notification server. Please do not reply to this email. If you would like to contact a member of our team, please&nbsp;<a href="mailto:support@genomenext.com">e-mail us</a>.&nbsp;This message and its contents are part of&nbsp;GenomeNext, LLC&nbsp;<a href="https://secure.genomenext.net/About/PrivacyPolicy">Privacy Policy</a>&nbsp;and&nbsp;<a href="https://secure.genomenext.net/About/TermsOfService">Terms of Service</a>.</span></span></p>

<p><object id="SILOBFWOBJECTID" style="width: 0px; height: 0px; display: block;"></object></p>
', NULL, N'ACTIVE', N'819af873-10bf-41c1-8a98-17b1402dc242', CAST(N'2014-11-07 01:21:29.293' AS DateTime), N'Reset Password                                    ', N'N', N'E-mail to send the users a link to reset their passwords.', N'Y')
INSERT [gn].[GNNotificationTopics] ([Id], [Code], [Format], [Sender], [Priority], [Subject], [Message], [MessageTemplatePath], [Status], [CreatedBy], [CreateDateTime], [Title], [IsSubscriptionOptional], [Description], [NotifyObjectCreator]) VALUES (4, N'USER_ACCOUNT_CONFIRM_EMAIL', N'EMAIL', N'noreply@genomenext.com                            ', N'NORMAL', N'Welcome! New user account created.', N'<p><img alt="[GenomeNext LLC logo]" src="https://secure.genomenext.net/Images/genomenextllclogo.jpg" style="float:left; margin:10px" /></p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<div style="background:#00cc99;border:1px solid #00cc99;padding:5px 10px;"><span style="font-size:36px"><span style="font-family:verdana,geneva,sans-serif"><span style="color:#FFFFFF">Welcome to GenomeNext!</span></span></span></div>

<p><span style="font-size:22px"><strong>A new user account has been created.</strong></span></p>

<div>
<p>Hello, {FirstName}</p>

<p>Thank you for taking the time to create an account with GenomeNext. To complete your user profile and change your password, click on the button below.</p>
</div>

<div style="width: 400px; border: 1px solid rgb(204, 204, 204); padding: 5px 10px; text-align: center; background: rgb(238, 238, 238);"><a href="{InvitationUrl}"><strong>Click Here to Complete Your Registration</strong></a></div>

<p>Now that you have created your account, you will be able to take advantage of our portal features, that include adding users to your organization, uploading samples, creating analyses, and managing your billing options.</p>

<div style="line-height: 20.7999992370605px;">
<p>We look forward to providing you with the fastest, most cost effective and accurate bioinformatics tools for your clinical and research needs.</p>

<div>
<p>Thank You,<br />
<br />
The&nbsp;GenomeNext&nbsp;Team<br />
<a href="http://www.genomenext.com">www.genomenext.com</a></p>

<table align="left" border="0" cellpadding="1" cellspacing="1" style="width:90px">
	<tbody>
		<tr>
			<td><a href="http://www.facebook.com/genomenext" style="text-decoration: none;"><img alt="[Facebook]" src="https://secure.genomenext.net/Images/fb_icon.png" style="border-width:0px; float:left; height:22px; width:22px" /></a></td>
			<td><a href="http://twitter.com/genomenext"><img alt="[Twitter]" src="https://secure.genomenext.net/Images/tw_icon.png" style="float:left; height:22px; width:22px" /></a></td>
			<td><a href="https://www.linkedin.com/company/3682901?trk=tyah&amp;trkInfo=idx%3A1-1-1%2CtarId%3A1415322712949%2Ctas%3Agenomenext"><img alt="[LinkedIn]" src="https://secure.genomenext.net/Images/in_icon.png" style="float:left; height:22px; width:22px" /></a></td>
		</tr>
	</tbody>
</table>

<p>&nbsp;&nbsp;&nbsp;<br />
&nbsp;</p>

<p><span style="color:rgb(169, 169, 169)"><span style="font-size:10px">This message was sent by an automated notification server. Please do not reply to this email. If you would like to contact a member of our team, please&nbsp;<a href="mailto:support@genomenext.com">e-mail us</a>.&nbsp;This message and its contents are part of&nbsp;GenomeNext, LLC&nbsp;<a href="https://secure.genomenext.net/About/PrivacyPolicy">Privacy Policy</a>&nbsp;and&nbsp;<a href="https://secure.genomenext.net/About/TermsOfService">Terms of Service</a>.</span></span></p>
</div>
</div>

<p><object id="SILOBFWOBJECTID" style="width: 0px; height: 0px; display: block;"></object></p>
', NULL, N'ACTIVE', N'819af873-10bf-41c1-8a98-17b1402dc242', CAST(N'2014-11-07 01:23:35.543' AS DateTime), N'Confirm e-mail account                            ', N'N', N'E-mail sent to the user once an account is created, requesting her/him to confirm the address entered.', N'Y')
INSERT [gn].[GNNotificationTopics] ([Id], [Code], [Format], [Sender], [Priority], [Subject], [Message], [MessageTemplatePath], [Status], [CreatedBy], [CreateDateTime], [Title], [IsSubscriptionOptional], [Description], [NotifyObjectCreator]) VALUES (5, N'USER_ACCOUNT_CHANGE_PASSWORD', N'EMAIL', N'noreply@genomenext.com                            ', N'NORMAL', N'Password changed.', N'<p><img alt="[GenomeNext LLC logo]" src="https://secure.genomenext.net/Images/genomenextllclogo.jpg" style="float:left; margin:10px" /></p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<div style="background:#00cc99;border:1px solid #00cc99;padding:5px 10px;"><span style="color:#FFFFFF"><span style="font-size:36px">Password Changed</span></span></div>

<p><span style="font-size:22px"><strong>GenomeNext Account Password Changed.</strong></span></p>

<div>
<p>You are receiving this email because you have updated your&nbsp;account password through our online portal.</p>

<p>If you have not performed this update or believe it is a mistake, please contact GenomeNext customer <a href="mailto:support@genomenext.com?subject=Password%20Change%20Support">support.</a></p>
</div>

<p>Thank You,<br />
<br />
The&nbsp;GenomeNext&nbsp;Team<br />
<a href="http://www.genomenext.com">www.genomenext.com</a></p>

<table align="left" border="0" cellpadding="1" cellspacing="1" style="line-height:20.799999237060547px; width:90px">
	<tbody>
		<tr>
			<td><a href="http://www.facebook.com/genomenext" style="text-decoration: none;"><img alt="[Facebook]" src="https://secure.genomenext.net/Images/fb_icon.png" style="border-width:0px; float:left; height:22px; width:22px" /></a></td>
			<td><a href="http://twitter.com/genomenext"><img alt="[Twitter]" src="https://secure.genomenext.net/Images/tw_icon.png" style="float:left; height:22px; width:22px" /></a></td>
			<td><a href="https://www.linkedin.com/company/3682901?trk=tyah&amp;trkInfo=idx%3A1-1-1%2CtarId%3A1415322712949%2Ctas%3Agenomenext"><img alt="[LinkedIn]" src="https://secure.genomenext.net/Images/in_icon.png" style="float:left; height:22px; width:22px" /></a></td>
		</tr>
	</tbody>
</table>

<p>&nbsp;&nbsp;&nbsp;<br />
&nbsp;</p>

<p><span style="color:rgb(169, 169, 169)"><span style="font-size:10px">This message was sent by an automated notification server. Please do not reply to this email. If you would like to contact a member of our team, please&nbsp;<a href="mailto:support@genomenext.com">e-mail us</a>.&nbsp;This message and its contents are part of&nbsp;GenomeNext, LLC&nbsp;<a href="https://secure.genomenext.net/About/PrivacyPolicy">Privacy Policy</a>&nbsp;and&nbsp;<a href="https://secure.genomenext.net/About/TermsOfService">Terms of Service</a>.</span></span></p>

<p><object id="SILOBFWOBJECTID" style="width: 0px; height: 0px; display: block;"></object></p>
', NULL, N'ACTIVE', N'819af873-10bf-41c1-8a98-17b1402dc242', CAST(N'2014-11-07 01:25:03.077' AS DateTime), N'Change Password                                   ', N'N', N'E-mail sent after the user changes his/her password.', N'Y')
INSERT [gn].[GNNotificationTopics] ([Id], [Code], [Format], [Sender], [Priority], [Subject], [Message], [MessageTemplatePath], [Status], [CreatedBy], [CreateDateTime], [Title], [IsSubscriptionOptional], [Description], [NotifyObjectCreator]) VALUES (6, N'ORG_ACCOUNT_REGISTRATION_COMPLETE', N'EMAIL', N'noreply@genomenext.com                            ', N'NORMAL', N'Registration Completed. Welcome to GenomeNext!', N'<p><img alt="[GenomeNext LLC logo]" src="https://secure.genomenext.net/Images/genomenextllclogo.jpg" style="float:left; margin:10px" /></p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<div style="background:#00cc99;border:1px solid #00cc99;padding:5px 10px;"><span style="font-size:36px"><span style="font-family:verdana,geneva,sans-serif"><span style="color:#FFFFFF">Welcome to GenomeNext!</span></span></span></div>

<p><span style="font-size:22px"><strong>The Registration Process is completed.</strong></span></p>

<div>
<p>Hello, {FirstName}</p>

<p>Thank you for taking the time to create an account with GenomeNext.</p>
</div>

<p>Now that you have created your account, you will be able to take advantage of our portal features, that include adding users to your organization, uploading samples, creating analyses, and managing your billing options.</p>

<div style="line-height: 20.7999992370605px;">
<p>We look forward to providing you with the fastest, most cost effective and accurate bioinformatics tools for your clinical and research needs.</p>

<div>
<p>Thank You,<br />
<br />
The&nbsp;GenomeNext&nbsp;Team<br />
<a href="http://www.genomenext.com">www.genomenext.com</a></p>

<table align="left" border="0" cellpadding="1" cellspacing="1" style="line-height:20.799999237060547px; width:90px">
	<tbody>
		<tr>
			<td><a href="http://www.facebook.com/genomenext" style="text-decoration: none;"><img alt="[Facebook]" src="https://secure.genomenext.net/Images/fb_icon.png" style="border-width:0px; float:left; height:22px; width:22px" /></a></td>
			<td><a href="http://twitter.com/genomenext"><img alt="[Twitter]" src="https://secure.genomenext.net/Images/tw_icon.png" style="float:left; height:22px; width:22px" /></a></td>
			<td><a href="https://www.linkedin.com/company/3682901?trk=tyah&amp;trkInfo=idx%3A1-1-1%2CtarId%3A1415322712949%2Ctas%3Agenomenext"><img alt="[LinkedIn]" src="https://secure.genomenext.net/Images/in_icon.png" style="float:left; height:22px; width:22px" /></a></td>
		</tr>
	</tbody>
</table>

<p>&nbsp;&nbsp;&nbsp;<br />
&nbsp;</p>

<p><span style="color:rgb(169, 169, 169)"><span style="font-size:10px">This message was sent by an automated notification server. Please do not reply to this email. If you would like to contact a member of our team, please&nbsp;<a href="mailto:support@genomenext.com">e-mail us</a>.&nbsp;This message and its contents are part of&nbsp;GenomeNext, LLC&nbsp;<a href="https://secure.genomenext.net/About/PrivacyPolicy">Privacy Policy</a>&nbsp;and&nbsp;<a href="https://secure.genomenext.net/About/TermsOfService">Terms of Service</a>.</span></span></p>
</div>
</div>

<p><object id="SILOBFWOBJECTID" style="width: 0px; height: 0px; display: block;"></object></p>
', NULL, N'ACTIVE', N'819af873-10bf-41c1-8a98-17b1402dc242', CAST(N'2014-11-07 01:25:57.327' AS DateTime), N'Organization Registration Completed               ', N'N', N'E-mail notifying the organitation main contact that the registration is completed.', N'Y')
INSERT [gn].[GNNotificationTopics] ([Id], [Code], [Format], [Sender], [Priority], [Subject], [Message], [MessageTemplatePath], [Status], [CreatedBy], [CreateDateTime], [Title], [IsSubscriptionOptional], [Description], [NotifyObjectCreator]) VALUES (7, N'ANALYSIS_COMPLETED_SUCCESSFULLY', N'EMAIL', N'noreply@genomenext.com                            ', N'NORMAL', N'Your analysis has completed!', N'<p><img alt="[GenomeNext LLC logo]" src="https://secure.genomenext.net/Images/genomenextllclogo.jpg" style="float:left; margin:10px" /></p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<div style="background:#00cc99;border:1px solid #00cc99;padding:5px 10px;"><span style="font-size:36px"><span style="font-family:verdana,geneva,sans-serif"><span style="color:#FFFFFF">Analysis completed!</span></span></span></div>

<p><span style="font-size:22px"><strong>Your analysis {AnalysisRequestDescription} has completed.</strong></span></p>

<p>Hello {CreatorName},&nbsp;</p>

<p>Your analysis completed on {EndDateTime} and generated {NumFiles} files. You can find more information and the result files by clicking on the following&nbsp;link:</p>

<div style="width: 400px; border: 1px solid rgb(204, 204, 204); padding: 5px 10px; text-align: center; background: rgb(238, 238, 238);"><a href="https://secure.genomenext.net/AnalysisRequests/Details/{AnalysisRequestId}"><strong>Click Here to see the results</strong></a></div>

<p>Thank You,<br />
<br />
The&nbsp;GenomeNext&nbsp;Team<br />
<a href="http://www.genomenext.com">www.genomenext.com</a></p>

<table align="left" border="0" cellpadding="1" cellspacing="1" style="line-height:20.799999237060547px; width:90px">
	<tbody>
		<tr>
			<td><a href="http://www.facebook.com/genomenext" style="text-decoration: none;"><img alt="[Facebook]" src="https://secure.genomenext.net/Images/fb_icon.png" style="border-width:0px; float:left; height:22px; width:22px" /></a></td>
			<td><a href="http://twitter.com/genomenext"><img alt="[Twitter]" src="https://secure.genomenext.net/Images/tw_icon.png" style="float:left; height:22px; width:22px" /></a></td>
			<td><a href="https://www.linkedin.com/company/3682901?trk=tyah&amp;trkInfo=idx%3A1-1-1%2CtarId%3A1415322712949%2Ctas%3Agenomenext"><img alt="[LinkedIn]" src="https://secure.genomenext.net/Images/in_icon.png" style="float:left; height:22px; width:22px" /></a></td>
		</tr>
	</tbody>
</table>

<p>&nbsp;&nbsp;&nbsp;<br />
&nbsp;</p>

<p><span style="color:rgb(169, 169, 169)"><span style="font-size:10px">This message was sent by an automated notification server. Please do not reply to this email. If you would like to contact a member of our team, please&nbsp;<a href="mailto:support@genomenext.com">e-mail us</a>.&nbsp;This message and its contents are part of&nbsp;GenomeNext, LLC&nbsp;<a href="https://secure.genomenext.net/About/PrivacyPolicy">Privacy Policy</a>&nbsp;and&nbsp;<a href="https://secure.genomenext.net/About/TermsOfService">Terms of Service</a>.</span></span></p>

<p><object id="SILOBFWOBJECTID" style="width: 0px; height: 0px; display: block;"></object></p>
', NULL, N'ACTIVE', N'819af873-10bf-41c1-8a98-17b1402dc242', CAST(N'2014-11-07 01:27:59.687' AS DateTime), N'Analysis completed                                ', N'N', N'An e-mail will be sent to you when your analysis has completed, providing a link to the result files and other information.', N'Y')


INSERT [gn].[GNNotificationTopics] ([Id], [Code], [Format], [Sender], [Priority], [Subject], [Message], [MessageTemplatePath], [Status], [CreatedBy], [CreateDateTime], [Title], [IsSubscriptionOptional], [Description], [NotifyObjectCreator]) VALUES (11, N'ANALYSIS_RETURNED_ERROR', N'EMAIL', N'noreply@genomenext.com                            ', N'NORMAL', N'Analysis completed with errors', N'

<p><img alt="[GenomeNext LLC logo]" src="https://secure.genomenext.net/Images/genomenextllclogo.jpg" style="float:left; margin:10px" /></p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<div style="background:#00cc99;border:1px solid #00cc99;padding:5px 10px;"><span style="font-size:36px"><span style="font-family:verdana,geneva,sans-serif"><span style="color:#FFFFFF">Analysis completed but&nbsp;returned errors</span></span></span></div>

<p><span style="font-size:22px"><strong>Your analysis <em>{AnalysisRequestDescription}</em> has completed, but returned errors.</strong></span></p>

<p>Hello {CreatorName},&nbsp;</p>

<p>Your analysis completed on {EndDateTime} but&nbsp;generated errors.&nbsp;You can find more information&nbsp;by clicking on the following&nbsp;link:</p>

<div style="width: 400px; border: 1px solid rgb(204, 204, 204); padding: 5px 10px; text-align: center; background: rgb(238, 238, 238);"><a href="https://secure.genomenext.net/AnalysisRequests/Details/{AnalysisRequestId}"><strong>Click Here to see the results</strong></a></div>

<p>Thank You,<br />
<br />
The&nbsp;GenomeNext&nbsp;Team<br />
<a href="http://www.genomenext.com">www.genomenext.com</a></p>

<table align="left" border="0" cellpadding="1" cellspacing="1" style="line-height:20.799999237060547px; width:90px">
	<tbody>
		<tr>
			<td><a href="http://www.facebook.com/genomenext" style="text-decoration: none;"><img alt="[Facebook]" src="https://secure.genomenext.net/Images/fb_icon.png" style="border-width:0px; float:left; height:22px; width:22px" /></a></td>
			<td><a href="http://twitter.com/genomenext"><img alt="[Twitter]" src="https://secure.genomenext.net/Images/tw_icon.png" style="float:left; height:22px; width:22px" /></a></td>
			<td><a href="https://www.linkedin.com/company/3682901?trk=tyah&amp;trkInfo=idx%3A1-1-1%2CtarId%3A1415322712949%2Ctas%3Agenomenext"><img alt="[LinkedIn]" src="https://secure.genomenext.net/Images/in_icon.png" style="float:left; height:22px; width:22px" /></a></td>
		</tr>
	</tbody>
</table>

<p>&nbsp;&nbsp;&nbsp;<br />
&nbsp;</p>

<p><span style="color:rgb(169, 169, 169)"><span style="font-size:10px">This message was sent by an automated notification server. Please do not reply to this email. If you would like to contact a member of our team, please&nbsp;<a href="mailto:support@genomenext.com">e-mail us</a>.&nbsp;This message and its contents are part of&nbsp;GenomeNext, LLC&nbsp;<a href="https://secure.genomenext.net/About/PrivacyPolicy">Privacy Policy</a>&nbsp;and&nbsp;<a href="https://secure.genomenext.net/About/TermsOfService">Terms of Service</a>.</span></span></p>

<p><object id="SILOBFWOBJECTID" style="width: 0px; height: 0px; display: block;"></object></p>
', NULL, N'ACTIVE', N'819af873-10bf-41c1-8a98-17b1402dc242', CAST(N'2014-11-07 01:27:59.687' AS DateTime), N'Analysis completed with errors', N'N', N'An e-mail will be sent to you when your analysis has completed with errors, providing a link to the results.', N'N');



INSERT [gn].[GNNotificationTopics] ([Id], [Code], [Format], [Sender], [Priority], [Subject], [Message], [MessageTemplatePath], [Status], [CreatedBy], [CreateDateTime], [Title], [IsSubscriptionOptional], [Description], [NotifyObjectCreator]) VALUES (8, N'ANALYSIS_STATUS_UPDATE_RETURNED_ERROR', N'EMAIL', N'noreply@genomenext.com                            ', N'NORMAL', N'Analysis Update: Error Received', N'<p><img alt="[GenomeNext LLC logo]" src="https://secure.genomenext.net/Images/genomenextllclogo.jpg" style="float:left; margin:10px" /></p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<p>&nbsp;</p>

<div style="background:#00cc99;border:1px solid #00cc99;padding:5px 10px;"><span style="font-size:36px"><span style="font-family:verdana,geneva,sans-serif"><span style="color:#FFFFFF">Analysis Update</span></span></span></div>

<p><span style="font-size:22px"><strong>Your analysis {AnalysisRequestName} returned an error.</strong></span></p>

<p>Hello {CreatorName},&nbsp;</p>

<p>The Analysis you created with description <strong>{AnalysisRequestName}</strong> has returned an error.</p>

<p>Request ID: {AnalysisRequestId}</p>

<p>Analysis Status: {AnalysisStatus}</p>

<p>Message: {AnalysisStatusMessage}</p>

<p>Percent Complete: {PercentComplete}</p>

<p>You can find more information by clicking on the following&nbsp;link:</p>

<div style="width: 400px; border: 1px solid rgb(204, 204, 204); padding: 5px 10px; text-align: center; background: rgb(238, 238, 238);"><a href="https://secure.genomenext.net/AnalysisRequests/Details/{AnalysisRequestId}"><strong>Click Here to go to the details page</strong></a></div>

<p>Thank You,<br />
<br />
The&nbsp;GenomeNext&nbsp;Team<br />
<a href="http://www.genomenext.com">www.genomenext.com</a></p>

<table align="left" border="0" cellpadding="1" cellspacing="1" style="line-height:20.799999237060547px; width:90px">
	<tbody>
		<tr>
			<td><a href="http://www.facebook.com/genomenext" style="text-decoration: none;"><img alt="[Facebook]" src="https://secure.genomenext.net/Images/fb_icon.png" style="border-width:0px; float:left; height:22px; width:22px" /></a></td>
			<td><a href="http://twitter.com/genomenext"><img alt="[Twitter]" src="https://secure.genomenext.net/Images/tw_icon.png" style="float:left; height:22px; width:22px" /></a></td>
			<td><a href="https://www.linkedin.com/company/3682901?trk=tyah&amp;trkInfo=idx%3A1-1-1%2CtarId%3A1415322712949%2Ctas%3Agenomenext"><img alt="[LinkedIn]" src="https://secure.genomenext.net/Images/in_icon.png" style="float:left; height:22px; width:22px" /></a></td>
		</tr>
	</tbody>
</table>

<p>&nbsp;&nbsp;&nbsp;<br />
&nbsp;</p>

<p><span style="color:rgb(169, 169, 169)"><span style="font-size:10px">This message was sent by an automated notification server. Please do not reply to this email. If you would like to contact a member of our team, please&nbsp;<a href="mailto:support@genomenext.com">e-mail us</a>.&nbsp;This message and its contents are part of&nbsp;GenomeNext, LLC&nbsp;<a href="https://secure.genomenext.net/About/PrivacyPolicy">Privacy Policy</a>&nbsp;and&nbsp;<a href="https://secure.genomenext.net/About/TermsOfService">Terms of Service</a>.</span></span></p>

<p><object id="SILOBFWOBJECTID" style="width: 0px; height: 0px; display: block;"></object></p>
', NULL, N'ACTIVE', N'819af873-10bf-41c1-8a98-17b1402dc242', CAST(N'2014-11-07 01:31:15.670' AS DateTime), N'Analysis returned an error                        ', N'N', N'An e-mail will be sent to you when your analysis returns an error.', N'Y')
SET IDENTITY_INSERT [gn].[GNNotificationTopics] OFF


INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (2, N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'BCC       ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:38:48.490' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (2, N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'CC        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:38:48.460' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (2, N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'TO        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-16 21:55:34.097' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (3, N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'CC        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:51:01.023' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (3, N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'TO        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:51:00.990' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (3, N'eb3e3558-9068-40c6-809b-8012c29a5d37', N'BCC       ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:51:01.053' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (4, N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'CC        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:34:10.177' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (4, N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'TO        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:34:10.163' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (4, N'eb3e3558-9068-40c6-809b-8012c29a5d37', N'BCC       ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:34:10.210' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (5, N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'BCC       ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:51:53.647' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (5, N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'CC        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:51:53.617' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (5, N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'TO        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:51:53.583' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (6, N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'BCC       ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:52:12.067' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (6, N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'CC        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:52:12.037' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (6, N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'TO        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:52:12.007' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (7, N'4aa47339-9413-423a-8304-940308d91e36', N'TO        ', N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-10-22 17:30:51.603' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (7, N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'TO        ', N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-10-22 17:30:51.683' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (7, N'7c797503-7367-4bd5-ab0e-bd759ddd2142', N'BCC       ', N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-10-22 17:30:51.730' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (7, N'8e73e782-0855-4785-9926-16f5a3829b8f', N'CC        ', N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-10-22 17:30:51.697' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (8, N'4aa47339-9413-423a-8304-940308d91e36', N'CC        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-23 19:20:28.233' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (8, N'6af2d704-6cdf-4f12-b61a-7915fb5d0415', N'TO        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-23 19:20:28.170' AS DateTime))
INSERT [gn].[GNNotificationTopicAddressees] ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType], [CreatedBy], [CreateDateTime]) VALUES (8, N'eb3e3558-9068-40c6-809b-8012c29a5d37', N'BCC       ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-23 19:20:28.247' AS DateTime))

INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (2, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'BCC       ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:38:48.490' AS DateTime), N'Y')
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (2, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'CC        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:38:48.473' AS DateTime), N'Y')
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (3, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'CC        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:51:01.023' AS DateTime), N'Y')
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (3, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'TO        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:51:00.990' AS DateTime), N'Y')
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (4, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'CC        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:34:10.193' AS DateTime), N'Y')
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (4, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'TO        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:34:10.163' AS DateTime), N'Y')
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (5, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'BCC       ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:51:53.647' AS DateTime), N'Y')
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (5, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'CC        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:51:53.617' AS DateTime), N'Y')
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (5, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'TO        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:51:53.600' AS DateTime), N'Y')
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (6, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'BCC       ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:52:12.083' AS DateTime), N'Y')
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (6, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'CC        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:52:12.053' AS DateTime), N'Y')
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (6, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'TO        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-22 23:52:12.023' AS DateTime), N'Y')
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (7, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'BCC       ', N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-10-22 17:30:51.743' AS DateTime), N'Y')
GO
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (7, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'CC        ', N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-10-22 17:30:51.697' AS DateTime), N'Y')
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (7, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'TO        ', N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-10-22 17:30:51.620' AS DateTime), N'Y')
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (8, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'CC        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-23 19:20:28.247' AS DateTime), N'Y')
INSERT [gn].[GNNotificationTopicSubscribers] ([GNNotificationTopicId], [GNContactId], [AddresseeType], [CreatedBy], [CreateDateTime], [IsSubscribed]) VALUES (8, N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'TO        ', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-23 19:20:28.233' AS DateTime), N'Y')

SET IDENTITY_INSERT [gn].[GNPaymentMethodTypes] ON 

INSERT [gn].[GNPaymentMethodTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (1, N'CHECK', N'Check', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:41:04.000' AS DateTime))
INSERT [gn].[GNPaymentMethodTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (2, N'CREDIT_CARD', N'Credit Card', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:41:40.000' AS DateTime))
INSERT [gn].[GNPaymentMethodTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (3, N'TRANSFER', N'Transfer', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:41:40.000' AS DateTime))
SET IDENTITY_INSERT [gn].[GNPaymentMethodTypes] OFF

SET IDENTITY_INSERT [gn].[GNProductTypes] ON 

INSERT [gn].[GNProductTypes] ([Id], [Name], [Description], [IsEligibleForDiscount], [CanSubscribe], [CreatedBy], [CreateDateTime]) VALUES (1, N'STORAGE', N'Storage Product Type', 0, 0, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:46:10.000' AS DateTime))
INSERT [gn].[GNProductTypes] ([Id], [Name], [Description], [IsEligibleForDiscount], [CanSubscribe], [CreatedBy], [CreateDateTime]) VALUES (2, N'ANALYSIS', N'Analysis Product Type', 1, 0, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:46:33.000' AS DateTime))
SET IDENTITY_INSERT [gn].[GNProductTypes] OFF

INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) VALUES (N'771b84dd-5d12-4112-9b94-517e5d35be9a', N'ANALYSIS_REQUEST_GENOME', N'Analysis Request for Whole Genome', 2, 0, 800, 800, 0, 0, N'N/A', 1, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:58:51.000' AS DateTime))
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) VALUES (N'ad5b82d7-bda5-44c3-acea-760a46b66297', N'STORAGE_S3_UPLOAD', N'Upload to S3 Storage (1 GB)', 1, 0, 0.03, 0.03, 0, 0, N'GB', 1, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-02 17:34:52.000' AS DateTime))
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) VALUES (N'c4f9f64b-274b-4871-92a6-7b39d6fe9a7c', N'ANALYSIS_REQUEST_EXOME', N'Analysis Request for Exome', 2, 0, 600, 600, 0, 0, N'N/A', 1, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:58:07.000' AS DateTime))
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) VALUES (N'04475d76-61d6-4793-b51a-f39d6a02c889', N'STORAGE_S3_CARRYOVER', N'Carry-Over of Files on S3 Storage', 1, 0, 0.03, 0.03, 0, 0, N'GB', 1, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-03 10:21:46.000' AS DateTime))
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) VALUES (N'8902a949-ac1a-4231-a442-f998a051ae50', N'STORAGE_S3_DELETE', N'Delete from S3 Storage', 1, 0, 0, 0, 1, 1, N'GB', 1, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-02 17:36:01.987' AS DateTime))
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) VALUES (N'54d3d28c-a1ab-4c92-afab-fa7e103a610f', N'STORAGE_S3_DOWNLOAD', N'Download from S3 Storage', 1, 0, 0.12, 0.12, 0, 0, N'GB', 1, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-03 09:22:45.000' AS DateTime))

SET IDENTITY_INSERT [gn].[GNSampleRelationshipTypes] ON 

INSERT [gn].[GNSampleRelationshipTypes] ([Id], [Name], [Gender], [MaxRelationships], [CreatedBy], [CreateDateTime]) VALUES (1, N'FATHER', N'M', 1, N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-09-30 19:01:21.003' AS DateTime))
INSERT [gn].[GNSampleRelationshipTypes] ([Id], [Name], [Gender], [MaxRelationships], [CreatedBy], [CreateDateTime]) VALUES (2, N'MOTHER', N'F', 1, N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-09-30 19:01:31.833' AS DateTime))
INSERT [gn].[GNSampleRelationshipTypes] ([Id], [Name], [Gender], [MaxRelationships], [CreatedBy], [CreateDateTime]) VALUES (3, N'SON', N'M', 99, N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-09-30 19:01:45.397' AS DateTime))
INSERT [gn].[GNSampleRelationshipTypes] ([Id], [Name], [Gender], [MaxRelationships], [CreatedBy], [CreateDateTime]) VALUES (4, N'-SELECT-', N'M', 0, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-11-06 00:27:24.280' AS DateTime))
INSERT [gn].[GNSampleRelationshipTypes] ([Id], [Name], [Gender], [MaxRelationships], [CreatedBy], [CreateDateTime]) VALUES (5, N'DAUGHTER', N'F', 99, N'6927dfad-da8f-4436-af7f-ca18bcf98ba8', CAST(N'2014-11-24 19:01:45.397' AS DateTime))
SET IDENTITY_INSERT [gn].[GNSampleRelationshipTypes] OFF
SET IDENTITY_INSERT [gn].[GNSampleTypes] ON 

INSERT [gn].[GNSampleTypes] ([Id], [Name], [TypeValue], [CreatedBy], [CreateDateTime]) VALUES (1, N'Whole Genome', N'GENOME', NULL, NULL)
INSERT [gn].[GNSampleTypes] ([Id], [Name], [TypeValue], [CreatedBy], [CreateDateTime]) VALUES (2, N'Exome', N'EXOME', NULL, NULL)
INSERT [gn].[GNSampleTypes] ([Id], [Name], [TypeValue], [CreatedBy], [CreateDateTime]) VALUES (3, N'Targ. Panel', N'EXOME', NULL, NULL)
SET IDENTITY_INSERT [gn].[GNSampleTypes] OFF

SET IDENTITY_INSERT [gn].[GNTransactionTypes] ON 

INSERT [gn].[GNTransactionTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (1, N'STORAGE_S3_UPLOAD', N'Upload to S3 Storage', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:04:12.000' AS DateTime))
INSERT [gn].[GNTransactionTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (2, N'STORAGE_S3_DELETE', N'Delete from S3 Storage', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:22:29.000' AS DateTime))
INSERT [gn].[GNTransactionTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (3, N'STORAGE_S3_DOWNLOAD', N'Download from S3 Storage', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:23:05.000' AS DateTime))
INSERT [gn].[GNTransactionTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (5, N'ANALYSIS_REQUEST_EXOME', N'Analysis Request for Exome', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:24:22.000' AS DateTime))
INSERT [gn].[GNTransactionTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (6, N'ANALYSIS_REQUEST_GENOME', N'Analysis Request for Whole Genome', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:24:45.000' AS DateTime))
INSERT [gn].[GNTransactionTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (10, N'STORAGE_S3_CARRYOVER', N'Carry-Over of Files on S3 Storage', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-03 10:22:15.000' AS DateTime))
SET IDENTITY_INSERT [gn].[GNTransactionTypes] OFF

SET ANSI_PADDING ON

GO