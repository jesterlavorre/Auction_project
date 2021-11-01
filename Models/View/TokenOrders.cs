using System.Collections.Generic;
using IepProjekat.Models.Database;
using X.PagedList;

namespace IepProjekat.Models.View
{
    public class TokenOrders{
        public IPagedList<TokenTransaction> transactions {get; set;}
        public int tokens{get; set;}

    }
}