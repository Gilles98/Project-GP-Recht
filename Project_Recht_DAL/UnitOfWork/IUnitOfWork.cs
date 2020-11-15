using Project_Recht_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht_DAL.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<Rechter> RechterRepo { get; }
        IRepository<Rechtbank> RechtbankRepo { get; }

        int Save();
    }
}
