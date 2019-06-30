using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TestModel;

namespace ConsoleApp1.LinqPratice
{
    class LinqFirstPratice
    {
        public static void Main()
        {
            #region C# Linq
            var A_Personlist = GenerateFixedPerson(1500, Guid.NewGuid().GetHashCode());
            var B_Personlist = GenerateFixedPerson(1500, Guid.NewGuid().GetHashCode());


            #region 交集

            //找出交集的資料
            // 方法1爆力法
            testFunc(() => TwoForeachCalculation(A_Personlist, B_Personlist), "TwoForeachCalculation");
            ////方法2UseContains
            testFunc(() => UseContains(A_Personlist, B_Personlist), "UseContains");
            ////方法3UseJoin
            testFunc(() => UseJoin(A_Personlist, B_Personlist), "UseJoin");
            ////方法4Intersect
            testFunc(() => UseIntersect(A_Personlist, B_Personlist), "UseIntersect");

            #endregion

            #region 聯集

            testFunc(() => UseConact(A_Personlist.Select(x => x.Salery).ToList()
                           , B_Personlist.Select(y => y.Salery).ToList()), "UseConact");
            
            testFunc(() => UseUnion(A_Personlist.Select(x => x.Salery).ToList()
                        , B_Personlist.Select(y => y.Salery).ToList()), "UseUnion");

            #endregion

            #region 分群

            testFunc(() => LinqTwoWhere(A_Personlist), "LinqTwoWhere");

            testFunc(() => LinqGroupBy(A_Personlist), "LinqGroupBy");

            #endregion

            #endregion


            Console.WriteLine("Brake point.");
        }

        #region Lesson 1

        static Tuple<int, int> LinqGroupBy(List<Person> personList)
        {
            var groupData = personList.GroupBy(x => x.gender).Select(x => new
            {
                gender = x.Key,
                count = x.Count(),
            }).ToDictionary(x=> x.gender,x=>x.count);

            return new Tuple<int, int>(groupData['M'], groupData['F']);
        }

        static Tuple<int, int> LinqTwoWhere(List<Person> personList)
        {
            var Man = personList.Where(x => x.gender == 'M').Count();
            var Female = personList.Where(x => x.gender == 'F').Count();

            return new Tuple<int, int>(Man, Female);
        }

        static List<int> UseConact(List<int> Aage, List<int> Bage)
        {
            return Aage.Concat(Bage).Distinct().ToList();
        }



        static List<int> UseUnion(List<int> Aage, List<int> Bage)
        {
            return Aage.Union(Bage).ToList();
        }


        static List<int> TwoForeachCalculation(List<Person> listA, List<Person> listB)
        {
            HashSet<int> val = new HashSet<int>();
            foreach (var item in listA)
            {
                foreach (var itemB in listB)
                {
                    if (item.Salery == itemB.Salery)
                    {
                        val.Add(item.Salery);
                    }
                }
            }

            return val.ToList();
        }

        static List<int> UseContains(List<Person> listA, List<Person> listB) {
            var resultA = listA.Select(x => x.Salery)
                          .Where(x => listB.Select(y => y.Salery).Contains(x)).OrderBy(x => x);

            return resultA.Distinct().ToList();
        }

        static List<int> UseJoin(List<Person> listA, List<Person> listB)
        {
            var resultB = listA.Join(listB,
                a => a.Salery,
                b => b.Salery,
                (a, b) => a.Salery).Distinct().ToList();
            return resultB;
        }

        static List<int> UseIntersect(List<Person> listA, List<Person> listB)
        {
            var result = listA.Select(x => x.Salery).Intersect(listB.Select(x => x.Salery)).ToList();
            return result;
        }

        /// <summary>
        /// 測試範例 - 執行Foreach
        /// </summary>
        /// <param name="list"></param>
        static void foreachFunc(List<Person> list)
        {
            foreach (var item in list)
            {
                if (item.Age > 150)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 測試範例 - 執行Linq Search
        /// </summary>
        /// <param name="list"></param>
        static void LinqFirstFunc(List<Person> list)
        {
            list.FirstOrDefault(x => x.Age == 150);
        }

        #endregion


        /// <summary>
        /// 測試Action的執行時間
        /// </summary>
        /// <param name="action">函式</param>
        /// <param name="methodName">函式名稱</param>
        static void testFunc(Action action,string methodName)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            action();
            sw.Stop();
            Console.WriteLine("Duration: {0:N0}ms,TestName:{1}", sw.ElapsedMilliseconds, methodName);
        }

        static List<Person> GenerateFixedPerson(int max,int seed = 9999)
        {
            const string characters = "assdjw2o2i1293122239859102kkkkllso11";

            var random = new Random(seed);
            return Enumerable.Repeat<Person>(Person.Empty, max).Select(s =>
            {
                Person p = new Person();
                p.Name = new string(Enumerable.Repeat(characters, 5).Select(x => x[random.Next(x.Length)]).ToArray());
                p.Age = random.Next(100);
                p.gender = random.Next(2) % 2 == 1 ? 'M' : 'F';
                p.Salery = random.Next(9999);
                return p;
            }).ToList();
        }
    }
}
