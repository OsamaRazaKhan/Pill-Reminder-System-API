//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pill_Reminder_System_api24.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Prescription
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Prescription()
        {
            this.P_Schedule = new HashSet<P_Schedule>();
        }
    
        public int id { get; set; }
        public string med_name { get; set; }
        public string med_type { get; set; }
        public string start_date { get; set; }
        public Nullable<int> doctor_id { get; set; }
        public Nullable<int> patient_id { get; set; }
        public string end_date { get; set; }
        public Nullable<int> no_dosage { get; set; }
        public Nullable<int> Weight { get; set; }
    
        public virtual Doctor Doctor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<P_Schedule> P_Schedule { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
