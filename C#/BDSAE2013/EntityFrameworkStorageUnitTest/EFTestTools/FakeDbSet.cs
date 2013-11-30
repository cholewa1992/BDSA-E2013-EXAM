using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFrameworkStorageUnitTest.EFTestTools
{
    public class FakeDbSet<T> : IDbSet<T> where T : class
    {
        readonly HashSet<T> _data;
        readonly IQueryable _query;
        private static FakeDbSet<T> _instance;

        public static FakeDbSet<T> GetInstace()
        {
            return _instance ?? (_instance = new FakeDbSet<T>());
        }

        public static void Reset()
        {
            _instance = null;
        }

        public FakeDbSet()
        {
            _data = new HashSet<T>();
            _query = _data.AsQueryable();
        }

        public T Add(T entity)
        {
            Console.WriteLine(entity);
            _data.Add(entity);
            return entity;
        }

        public T Attach(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            throw new NotImplementedException();
        }

        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public virtual T Find(params object[] keyValues)
        {
            throw new NotImplementedException(
               "Derive from FakeDbSet and override Find");
        }

        public System.Collections.ObjectModel.ObservableCollection<T> Local
        {
            get
            {
                return new
                  System.Collections.ObjectModel.ObservableCollection<T>(_data);
            }
        }

        public T Remove(T entity)
        {
            _data.Remove(entity);
            return entity;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public Type ElementType
        {
            get { return _query.ElementType; }
        }

        public Expression Expression
        {
            get { return _data.AsQueryable().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return _data.AsQueryable().Provider; }
        }
    }
}
