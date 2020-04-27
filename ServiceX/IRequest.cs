using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Collections;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ServiceX
{
    [ServiceContract]
    public interface IRequest
    {

        [OperationContract]
        int getCountRefresh();
        [OperationContract]
        List<Contract> getContracts();
        [OperationContract]
        void refreshStationList(string ville);
        [OperationContract]
        String getStation(String keyword);
        [OperationContract]
        List<VelibStation> getAllStations();
        [OperationContract] 
        void closeConnection();
    }

    // Utilisez un contrat de données comme indiqué dans l'exemple ci-après pour ajouter les types composites aux opérations de service.
    // Vous pouvez ajouter des fichiers XSD au projet. Une fois le projet généré, vous pouvez utiliser directement les types de données qui y sont définis, avec l'espace de noms "ServiceX.ContractType".
}
