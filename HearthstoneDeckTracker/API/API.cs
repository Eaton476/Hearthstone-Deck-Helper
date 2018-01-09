using HearthstoneDeckTracker.Model;
using unirest_net.http;
using unirest_net.request;

namespace HearthstoneDeckTracker.Data
{
    public static class API
    {
        const string path = "https://omgvamp-hearthstone-v1.p.mashape.com/cards";

        public static void GetAllCardData()
        {
            //HttpResponse<CardCollection> request = Unirest.get(path)
            //    .header("X-Mashape-Key", "9EAUhPT7DEmshT6PKdyGk9TNI4o3p1OsVzPjsncGteFRBezHnc")
            //    .asJson<CardCollection>();
        }


    }
}
