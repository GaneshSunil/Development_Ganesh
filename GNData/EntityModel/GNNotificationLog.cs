
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace GenomeNext.Data.EntityModel
{

using System;
    using System.Collections.Generic;
    
public partial class GNNotificationLog
{

    public long Id { get; set; }

    public int GNNotificationTopicId { get; set; }

    public string GNNotificationTopicCode { get; set; }

    public string Addressee { get; set; }

    public string Sender { get; set; }

    public string Subject { get; set; }

    public string Message { get; set; }

    public string Source { get; set; }

    public string NotificationServiceResponse { get; set; }

    public Nullable<System.Guid> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }



    public virtual GNNotificationTopic GNNotificationTopic { get; set; }

}

}
