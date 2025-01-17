using System;

namespace LILI_VMS.Models.ReportsViewModels
{
    public class VehicleMovementBillReport
    {
        public int Id { get; set; }
        public DateTime LoadingDate { get; set; }
        public DateTime BillDate { get; set; }
        public Decimal KPL { get; set; }
        public string DriverName { get; set; }
        public string Weight { get; set; }
        public string RegNo { get; set; }
        public decimal ClosingKM { get; set; }
        public decimal RunOfKm { get; set; }
        public decimal StartingKm { get; set; }
        public decimal TotalLoadMt { get; set; }
        public string Customer {  get; set; }
        public string CustomerAddress { get; set; }
        public string ExpenseName { get; set; }
        public string Unit {  get; set; }
        public string GPNo { get; set; }
        public decimal TotalPrice { get; set; }
        public string Comments { get; set; }
    }
}
