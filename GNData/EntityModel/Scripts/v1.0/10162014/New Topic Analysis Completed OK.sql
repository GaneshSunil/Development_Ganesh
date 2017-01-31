
INSERT INTO [gn].[GNNotificationTopics]
           ([Code],[Format],[Sender],[Priority],[Subject],[MessageTemplatePath],[Status],[CreatedBy],[CreateDateTime],[Title],[IsSubscriptionOptional],[Description],[NotifyObjectCreator],[Message])
     VALUES
           ('ANALYSIS_COMPLETED_SUCCESSFULLY','EMAIL','telma.frege@genomenext.com','NORMAL','Your analysis has completed!',NULL,'ACTIVE','6927dfad-da8f-4436-af7f-ca18bcf98ba8'
           ,NULL ,'Analysis completed','N','An e-mail will be sent to you when your analysis has completed, providing a link to the result files and other information.','N'
           ,'<p><img alt="[GenomeNext LLC logo]" src="https://secure.genomenext.net/Images/genomenextllclogo.jpg" style="float:left; margin:10px" /></p>

<div style="background:#00cc99;border:1px solid #00cc99;padding:5px 10px;"><span style="font-size:36px"><span style="font-family:verdana,geneva,sans-serif"><span style="color:#FFFFFF">Analysis completed!</span></span></span></div>

<p><span style="font-size:22px"><strong>Your analysis {AnalysisRequestDescription} has completed.</strong></span></p>

<p>Hello {CreatorName},&nbsp;</p>

<p>Your analysis completed on {EndDateTime} and generated {NumFiles} files. You can find more information and the result files by clicking on this link:</p>

<div style="width: 400px; border: 1px solid rgb(204, 204, 204); padding: 5px 10px; text-align: center; background: rgb(238, 238, 238);"><a href="{AnalysisDetailUrl}"><strong>Click Here to see the results</strong></a></div>

<p>Thank You,<br />
The GenomeNext Team<br />
<a href="http://www.genomenext.com">www.genomenext.com</a></p>

<p><a href="http://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[Facebook]" src="https://secure.genomenext.net/Images/fb_icon.png" style="border:0px; float:left; height:22px; width:22px" /></a>&nbsp;&nbsp;&nbsp;<a href="https://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[Twitter]" src="https://secure.genomenext.net/Images/tw_icon.png" style="float:left; height:22px; width:22px" /></a>&nbsp;&nbsp;&nbsp;<a href="https://www.facebook.com/genomenext" style="text-decoration:none"><img alt="[LinkedIn]" src="https://secure.genomenext.net/Images/in_icon.png" style="float:left; height:22px; width:22px" /></a></p>

<p><span style="color:#A9A9A9"><span style="font-size:10px">This message was sent by an automated notification server. Please do not reply to this email. If you would like to contact a member of our team, please <u>email &lt;insert email&gt;</u> or call us at 888-555-3333.&nbsp;</span></span><span style="color:#A9A9A9"><span style="font-size:10px">To change your notification preferences, <u>click here.</u></span></span></p>
')GO