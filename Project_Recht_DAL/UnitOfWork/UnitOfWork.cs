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
        private IRepository<Rechtzaak> _rechtzaakRepo;
        private IRepository<Aanklager> _aanklagerRepo; 
        private IRepository<Beklaagde> _beklaagdeRepo;
        private IRepository<RechtzaakAanklager> _rechtzaakAanklagerRepo; 
        private IRepository<RechtzaakBeklaagde> _rechtzaakBeklaagdeRepo;
        private IRepository<Jury> _juryRepo;
        private IRepository<Jurylid> _jurLidRepo;
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

        public IRepository<Rechtzaak> RechtzaakRepo
        {
            get
            {
                if (_rechtzaakRepo == null)
                {
                    _rechtzaakRepo = new Repository<Rechtzaak>(this.Context);
                }
                return _rechtzaakRepo;
            }
        }
        public IRepository<Aanklager> AanklagerRepo
        {
            get
            {
                if (_aanklagerRepo == null)
                {
                    _aanklagerRepo = new Repository<Aanklager>(this.Context);
                }
                return _aanklagerRepo;
            }
        }
        public IRepository<Beklaagde> BeklaagdeRepo
        {
            get
            {
                if (_beklaagdeRepo == null)
                {
                    _beklaagdeRepo = new Repository<Beklaagde>(this.Context);
                }
                return _beklaagdeRepo;
            }
        }
        public IRepository<RechtzaakAanklager> RechtzaakAanklagerRepo
        {
            get
            {
                if (_rechtzaakAanklagerRepo == null)
                {
                    _rechtzaakAanklagerRepo = new Repository<RechtzaakAanklager>(this.Context);
                }
                return _rechtzaakAanklagerRepo;

            }
        }
        public IRepository<RechtzaakBeklaagde> RechtzaakBeklaagdeRepo
        {
            get
            {
                if (_rechtzaakBeklaagdeRepo == null)
                {
                    _rechtzaakBeklaagdeRepo = new Repository<RechtzaakBeklaagde>(this.Context);
                }
                return _rechtzaakBeklaagdeRepo;
            }
        }
        public IRepository<Jury> JuryRepo
        {
            get
            {
                if (_juryRepo == null)
                {
                    _juryRepo = new Repository<Jury>(this.Context);
                }
                return _juryRepo;
            }
        }
        public IRepository<Jurylid> JurylidRepo
        {
            get
            {
                if (_jurLidRepo == null)
                {
                    _jurLidRepo = new Repository<Jurylid>(this.Context);
                }
                return _jurLidRepo;
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
