using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoMilestoneV2.Models
{
    public class StaffCarUploadModel
    {
        public string name { get; set; }
        public string model { get; set; }

        public HttpPostedFileBase carPicture { get; set; }
    }

    public class CustomerBookingModel
    {
        public int customer_booking_id { get; set; }
        public string userId { get; set; }
        public int vehicle_id { get; set; }
        public string isAccepted { get; set; }
        public DateTime to_date { get; set; }
        public DateTime from_date { get; set; }
        public string pickup_location { get; set; }
        public string dropoff_location { get; set; }
    }

    public class StaffViewCustomerBookingInDashboard
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public int customer_booking_id { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public int vehicle_id { get; set; }

    }

    public class CustomerModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

    }
    public class CustomerBookingModelForStaff
    {
        public int customer_booking_id { get; set; }
        public int vehicle_id { get; set; }
        public DateTime to_date { get; set; }
        public DateTime from_date { get; set; }

    }

    public class CustomerBookingLocationForStaff
    {
        public string longitude { get; set; }
        public string latitude { get; set; }
    }

    public class CustomerResponse
    {
        public String response { get; set; }
        public Double distance { get; set; }
        public Double price { get; set; }
        public String message { get; set; }
    }
}