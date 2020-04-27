
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ConsoleClient.ServiceX;
//using VelibLibrary;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            
            RequestClient req = new RequestClient();
            Contract[] cList = req.getContracts();
            
            foreach (Contract c in cList)
                Console.WriteLine(c.name);
            Console.Write("Choose a city:\t");
            string city = Console.ReadLine();

            req.refreshStationList(city);
            VelibStation[] stations = req.getAllStations();
            foreach (VelibStation v in stations)
                Console.WriteLine(v.name);
            
            
            Console.Write("Choose a station:\t");
            string stationName = Console.ReadLine();

            while (stationName != "exit")
            {
                string stationInfos = req.getStation(stationName);
                Console.WriteLine(stationInfos);
                Console.WriteLine("coutRefresh=\t" + req.getCountRefresh());
                Console.Write("Choose a station:\t");
                stationName = Console.ReadLine();
            }



            req.closeConnection();

            Console.ReadLine();
            

            /*
                MaJ dynamiquement toutes les x minutes pour avoir une liste de stations a jour
                select ville, ensuite select station, ensuite get les infos de la station
             */
        }
    }
}
