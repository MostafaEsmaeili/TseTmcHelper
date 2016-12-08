using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameWork.Logger;
using FrameWork.Utilities;
using TseTmc.Domain;
using TseTmc.Domain.Enum;
using TseTmc.Domain.TseTmcObject;
using TseTmcFacade.TseTmcService;

namespace TseTmcFacade
{
    class TseTmcFacade
    {
        private static TseTmcFacade _instance;
        private TsePublicV2SoapClient TseService = new TsePublicV2SoapClient();
        public CustomLogger Logger => new CustomLogger(GetType().FullName);

        private const string UserName = "emofid.com";
        private const string Password = "emofid";
        public static TseTmcFacade Instance => (_instance ?? (_instance = new TseTmcFacade()));
     
        public List<Insrument> GetInsCode(Flow flow)
        {
            try
            {
                var product = TseService.Instrument(UserName, Password, ((int)flow).SafeByte()).Tables[0].ToList<Insrument>();
                return product;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
                throw;
            }
        }
        public List<IndexB1LastDayLastData> GetIndexDailyDetail(Flow flow)
        {
            try
            {
                var index =
                    TseService.IndexB1LastDayLastData(UserName, Password, ((int)flow).SafeByte()).Tables[0]
                        .ToList<IndexB1LastDayLastData>();
                index.ForEach(x => x.MarketCode = MarketCodeMapperExchange(flow));
                return index;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
                throw;
            }
        }
        public List<IndexB2> GetIndexHistoricalDetail(int date)
        {
            try
            {
                var index =
                    TseService.IndexB2(UserName, Password, date).Tables[0]
                        .ToList<IndexB2>();
                return index;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
                throw;
            }
        }
        public List<TradeLastDay> GetSymbolsTradesIntraDay(Flow flow)
        {
            try
            {
                var index =
                    TseService.TradeLastDay(UserName, Password, ((int)flow).SafeByte()).Tables[0]
                        .ToList<TradeLastDay>();
                return index;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
                throw;
            }
        }

        public List<InstTrade> GetSymbolTrades(long insCode, int fromDate, int toDate)
        {
            try
            {
                var index =
                    TseService.InstTrade(UserName, Password, insCode, fromDate, toDate).Tables[0]
                        .ToList<InstTrade>();
                return index;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
                throw;
            }
        }

        public List<TradeOneDay> GetSymbolsTradesEndOfDay(int inDate, Flow flow)
        {
            try
            {
                var index =
                    TseService.TradeOneDay(UserName, Password, inDate, ((int)flow).SafeByte()).Tables[0]
                        .ToList<TradeOneDay>();
                return index;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
                throw;
            }
        }


        public List<MarketActivityHistorical> GetMarketDetailHistorical(int fromDate, int toDate)
        {
            try
            {
                var index =
                    TseService.MarketActivityDaily(UserName, Password, fromDate, toDate).Tables[0]
                        .ToList<MarketActivityHistorical>();
                index.ForEach(x => x.MarketCode = PortfolioStockExchange.TSE);
                return index;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
                throw;
            }
        }
        public decimal GetMarketValueByDate(int date)
        {
            try
            {
                var index =
                    TseService.MarketValueByDate(UserName, Password, date);

                return index;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
                throw;
            }
        }
        public List<MarketActivityDaily> GetMarketDetailDaily(Flow flow)
        {
            try
            {
                var index =
                    TseService.MarketActivityLast(UserName, Password, ((int)flow).SafeByte()).Tables[0]
                        .ToList<MarketActivityDaily>();
                index.ForEach(x => x.MarketCode = MarketCodeMapperExchange(flow));
                return index;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
                throw;
            }
        }

        public PortfolioStockExchange MarketCodeMapperExchange(Flow flow)
        {
            switch (flow)
            {
                case Flow.Bourse:
                case Flow.Future:
                case Flow.General:
                    return PortfolioStockExchange.TSE;
                case Flow.BaseOTC:
                case Flow.OTC:
                    return PortfolioStockExchange.IFB;
                default:
                    return PortfolioStockExchange.Unknown;
            }
        }

    }
}
}
