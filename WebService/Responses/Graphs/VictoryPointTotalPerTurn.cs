﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Dominion;
using Dominion.Strategy;
using Dominion.Data;

namespace Program.WebService
{
    class VictoryPointTotalPerTurn
      : PerTurnGraph,
        IRequestWithJsonResponse
    {
        public object GetResponse(WebService service)
        {
            StrategyComparisonResults comparisonResults = service.GetResultsFor(this);

            return GetLineGraphData(comparisonResults,
                "Victory Point Total Per Turn",
                comparisonResults.statGatherer.victoryPointTotal);
        }
    }
}
