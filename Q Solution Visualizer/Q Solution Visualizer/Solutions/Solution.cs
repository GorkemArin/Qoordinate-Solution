using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_Solution_Visualizer.Solutions
{
    public class Solution
    {
        Dictionary<string, string[]> orders = new Dictionary<string, string[]>();

        public Solution()
        {

        }

        public void AddNewOrder(string teamName, string[] buildingsOrder)
        {
            orders.Add(teamName, buildingsOrder);
        }

        public string[] GetOrderByKey(string teamName)
        {
            if (!orders.ContainsKey(teamName))
            {
                return new string[0];
            }

            return orders[teamName];
        }

        public string[] GetTeamNames()
        {
            return orders.Keys.ToArray();
        }

    }
}
