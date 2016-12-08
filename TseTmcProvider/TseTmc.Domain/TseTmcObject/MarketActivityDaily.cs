namespace TseTmc.Domain.TseTmcObject
{
   public class MarketActivityDaily
    {
       public string LastDataDate { get; set; }
       public decimal IndexLastValue { get; set; }
       public decimal IndexChange { get; set; }
       public string MarketActivityDeven { get; set; }
       public string MarketActivityHeven { get; set; }
       public int MarketActivityZTotTran { get; set; }
       public decimal MarketActivityQTotCap { get; set; }
       public decimal MarketActivityQTotTran { get; set; }
       public string MarketState { get; set; }
       public decimal MarketValue { get; set; }
       public PortfolioStockExchange MarketCode { get; set; }
    }
}
