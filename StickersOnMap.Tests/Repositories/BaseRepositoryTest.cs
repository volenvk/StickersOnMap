namespace StickersOnMap.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using AutoMapper;
    using Core.Infrastructure.Pages;
    using NUnit.Framework;
    using WEB.Infrastructure.MappingProfiles;

    public abstract class BaseRepositoryTest
    {
        protected DataSet CommonTestDataSet;
        private Fixture _fixture;
        private static readonly Lazy<MapperConfiguration> _mapperConfiguration =
            new Lazy<MapperConfiguration>(() => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfileSticker());
            }));
        
        protected static IMapper CreateSingletonMapper()
        {
            var mapper = _mapperConfiguration.Value.CreateMapper();
            mapper.ConfigurationProvider.CompileMappings();
            return mapper;
        }

        protected class DataSet
        {
            public int ElementCount { get; }
            public int Page { get; }
            public int RandomId => _random.Next(1, int.MaxValue);
            public string RandomName => $"TestName{Guid.NewGuid().ToString()}";
            private readonly Random _random;

            public DataSet()
            {
                ElementCount = 10;
                Page = 1;
                _random = new Random();
            }
            
            public DataSet(int? count = null, int? page = null) : this()
            {
                ElementCount = count ?? ElementCount;
                if (ElementCount <= 0 )
                    throw new ArgumentException("Количество записей для коллекций не может быть нулевым! Воспользуйтесь другим условием.");
                
                Page = page ?? Page;
                if (Page <= 0 )
                    throw new ArgumentException("Количество возвращаемых страниц не может быть нулевым! Воспользуйтесь другим условием.");
            }
        }
        
        protected class AggregateAction<T>
        {
            private Queue<IEnumerable<T>> _queue;
            private HashSet<Delegate> _func;
            private HashSet<Delegate> _funcPaged;

            public AggregateAction()
            {
                _queue = new Queue<IEnumerable<T>>();
                _func = new HashSet<Delegate>();
                _funcPaged = new HashSet<Delegate>();
            }
            
            public void AddAct<TQ>(Func<TQ, IEnumerable<T>> func)
            {
                _func.Add(func);
            }

            public void AddActWithPaged<TQ>(Func<TQ, int?, IPagedList<T>> func)
            {
                _funcPaged.Add(func);
            }

            public void Invoke<TQ>(TQ arg, int? page = 1)
            {
                foreach (var fDelegate in _func)
                {
                    if (fDelegate?.DynamicInvoke(arg) is IEnumerable<T> result)
                        _queue.Enqueue(result);
                }

                foreach (var fDelegate in _funcPaged)
                {
                    if (fDelegate?.DynamicInvoke(arg, page) is IPagedList<T> pageResult)
                        _queue.Enqueue(pageResult.Data);
                }
            }
            
            public IEnumerable<IEnumerable<T>> Results()
            {
                if (!_queue.Any())
                    yield break;

                while (_queue.Any())
                    yield return _queue.Dequeue();
            }
        }

        [SetUp]
        public void SetUp()
        {
            CommonTestDataSet = new DataSet();
            _fixture = new Fixture();
        }
        
        protected T CreateElement<T>() where T : class => _fixture.Build<T>().Create();

        protected IQueryable<T> CreateCollectionWithLastModifyElement<T>(Action<T> mod, int? count = null) where T : class
        {
            var collection = CreateCollection<T>(count);
            var target = collection.Last();
            mod?.Invoke(target);
            
            return collection.AsQueryable();
        }

        protected IQueryable<T> CreateCollectionWithModifyElements<T>(Action<T> mod, int? count = null) where T : class
        {
            var collection = CreateCollection<T>(count);
            foreach (var element in collection)
                mod?.Invoke(element);
            return collection.AsQueryable();
        }

        protected IQueryable<T> CreateCollection<T>(int? count = null) where T : class =>
            _fixture.Build<T>().CreateMany(count ?? CommonTestDataSet.ElementCount).AsQueryable();

        protected static class Assert<T, TR>
        {
            public static void AreEqual(T expected, TR result)
            {
                Assert.IsNotNull(result);
                
                foreach (var propertyInfo in typeof(T).GetProperties())
                {
                    var propertyName = propertyInfo.Name;
                    if (typeof(T) != typeof(TR))
                        propertyName = MapByName(propertyName);

                    var resultProperty = typeof(TR).GetProperties().FirstOrDefault(f => f.Name.Equals(propertyName));
                    
                    if (resultProperty == null)
                    {
                        Console.WriteLine($"Свойство {propertyName} не найдено у объекта {typeof(TR).Name}");
                        continue;
                    }

                    var resultPropertyName = resultProperty.Name;
                    if (resultPropertyName.Equals("ChangeDate") || resultPropertyName.Equals("ChangeAuthor"))
                    {
                        Console.WriteLine($"Свойство {resultPropertyName} проигнорировано у объекта {typeof(TR).Name}");
                        continue;
                    }
                    
                    Assert.AreEqual(propertyInfo.GetValue(expected), resultProperty.GetValue(result));
                }
            }
            
            public static void AreEqual(T expected, IEnumerable<TR> result)
            {
                Assert.IsNotNull(result, "Результат не определен");
                Assert.IsTrue(result.Any(), "Коллекция пуста");
                
                foreach (var item in result)
                {
                    AreEqual(expected, item);
                }
            }

            private static string MapByName(string name)
            {
                switch (name)
                {
                    default:
                        return name;
                }
            }
        }
    }
}