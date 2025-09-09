namespace GisZhkhAdmin.ViewModels
{
    public class ContractDashboardViewModel
    {
        public int TotalContracts { get; set; }
        public int LoadedToGis { get; set; }
        public int NotLoadedToGis { get; set; }
        public int WithErrors { get; set; }
        public int Active { get; set; }
    }
}