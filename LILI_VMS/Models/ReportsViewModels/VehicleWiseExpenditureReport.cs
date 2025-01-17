namespace LILI_VMS.Models.ReportsViewModels
{
    public class VehicleWiseExpenditureReport
    {
        public int Id { get; set; }
        public string VehicleCode { get; set; }
        public string RegistrationNo { get; set; }
        public string Owner { get; set; }
        public decimal TotalLoadMT { get; set; }
        public decimal RunOfKM { get; set; }
        public decimal LtrOfFuel { get; set; }
        public decimal FuelCost { get; set; }
        public decimal DriverAllowance { get; set; }
        public decimal HelperAllowance { get; set; }
        public decimal Toll { get; set; }
        public decimal Subscription { get; set; }
        public decimal NightHold { get; set; }
        public decimal LoadBill { get; set; }
        public decimal UnloadBill { get; set; }
        public decimal CaseFile { get; set; }
        public decimal Police { get; set; }
        public decimal Challan { get; set; }
        public decimal Repair { get; set; }
        public decimal Others { get; set; }
        public decimal TotalCost { get; set; }
        public decimal CostPerKg { get; set; }
        public decimal CostPerKm { get; set; }
    }
}
