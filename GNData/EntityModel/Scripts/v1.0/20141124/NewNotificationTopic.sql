
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

