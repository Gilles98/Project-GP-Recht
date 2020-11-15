using Project_Recht_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht_DAL.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private IRepository<Rechter> _rechterRepo;
        private IRepository<Rechtbank> _rechtbankRepo;
        public UnitOfWork(RechtContext context)
        {
            Context = context;
        }

        private RechtContext Context { get; }

        public IRepository<Rechter> RechterRepo
        {
            get
            {
                if (_rechterRepo == null)
                {
                    _rechterRepo = new Repository<Rechter>(this.Context);
                }
                return _rechterRepo;
            }
        }


        public IRepository<Rechtbank> RechtbankRepo
        {
            get
            {
                if (_rechtbankRepo == null)
                {
                    _rechtbankRepo = new Repository<Rechtbank>(this.Context);
                }
                return _rechtbankRepo;
            }
        }


        public void Dispose()
        {
            Context.Dispose();
        }

        public int Save()
        {
            return Context.SaveChanges();
        }
    }
}
