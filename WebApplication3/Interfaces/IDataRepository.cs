using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.Interfaces
{
    public interface IDataRepository
    {
        string LoginUser(string userId, string password);
        string InsertBoatInformation(clsBoat objBoat);
        string retrievBoatStatus(clsBoat objBoat);
        int getReq();
    }
}
