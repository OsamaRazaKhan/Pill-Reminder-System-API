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
    
    public partial class Dispenser2
    {
        public int disp_id { get; set; }
        public Nullable<int> sun_morn { get; set; }
        public Nullable<int> sun_noon { get; set; }
        public Nullable<int> sun_even { get; set; }
        public Nullable<int> sun_night { get; set; }
        public Nullable<int> mon_morn { get; set; }
        public Nullable<int> mon_noon { get; set; }
        public Nullable<int> mon_even { get; set; }
        public Nullable<int> mon_night { get; set; }
        public Nullable<int> tue_morn { get; set; }
        public Nullable<int> tue_noon { get; set; }
        public Nullable<int> tue_even { get; set; }
        public Nullable<int> tue_night { get; set; }
        public Nullable<int> wed_morn { get; set; }
        public Nullable<int> wed_noon { get; set; }
        public Nullable<int> wed_even { get; set; }
        public Nullable<int> wed_night { get; set; }
        public Nullable<int> thr_morn { get; set; }
        public Nullable<int> thr_noon { get; set; }
        public Nullable<int> thr_even { get; set; }
        public Nullable<int> thr_night { get; set; }
        public Nullable<int> fri_morn { get; set; }
        public Nullable<int> fri_noon { get; set; }
        public Nullable<int> fri_even { get; set; }
        public Nullable<int> fri_night { get; set; }
        public Nullable<int> sat_morn { get; set; }
        public Nullable<int> sat_noon { get; set; }
        public Nullable<int> sat_even { get; set; }
        public Nullable<int> sat_night { get; set; }
        public Nullable<int> patient_id { get; set; }
    
        public virtual Patient Patient { get; set; }
    }
}
