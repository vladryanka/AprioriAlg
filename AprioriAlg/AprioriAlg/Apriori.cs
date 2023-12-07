using System;
using System.Linq;
using System.Collections.Generic;

namespace AprioriAlg
{
    class Apriori
    {
        private static int support; 
        private static double confidence; 
        private static char[] item_Split = { ',' }; 
        private static string itemSplit = ",";
        private static String CON = "->"; 
        private static List<String> transList = new List<String>(); 
        public Apriori(int min_sup, double min_conf) // Конструктор, в котором начальные пороги поддержки и доверия инициализируются
        {
            transList.Add("apple,banana,orange,pear,");
            transList.Add("apple,banana,orange,");
            transList.Add("cake,orange,");
            transList.Add("milk,pear,");
            transList.Add("apple,cake,orange,");
            transList.Add("apple,pear,");
            transList.Add("pizza,orange,pear,");
            transList.Add("apple,pear,");
            transList.Add("apple,banana,orange,pear,");
            transList.Add("apple,orange,pear,");
            support = min_sup;
            confidence = min_conf;
        }

        public List<String> AprioriShow() {
            return transList;
        }
        private Dictionary<String, int> getItem()
        {
            Dictionary<String, int> mItem = new Dictionary<String, int>();
            Dictionary<String, int> Items = new Dictionary<String, int>(); 
            foreach (String trans in transList)
            {
                String[] items = trans.Split(item_Split, StringSplitOptions.RemoveEmptyEntries); 
                foreach (String item in items)
                {
                    int count;
                    if (mItem.ContainsKey(item + itemSplit) == true)
                    {
                        count = mItem[item + itemSplit];
                        mItem.Remove(item + itemSplit);
                        mItem.Add(item + itemSplit, count + 1);
                    }
                    else
                    {
                        mItem.Add(item + itemSplit, 1);
                    }
                }
            }
            List<String> keySet = mItem.Keys.ToList();
            foreach (String key in keySet)
            {
                int count = mItem[key];
                if (count >= support)
                {
                    Items.Add(key, count);
                }
            }
            return Items;
        }
        private Dictionary<String, int> getCollection(Dictionary<String, int> item) 
        {
            Dictionary<String, int> dateCollection = new Dictionary<String, int>();
            List<String> itemSet1 = item.Keys.ToList();
            List<String> itemSet2 = item.Keys.ToList();
            foreach (String item1 in itemSet1)
            {
                foreach (String item2 in itemSet2)
                {
                    String[] tmp1 = item1.Split(item_Split, StringSplitOptions.RemoveEmptyEntries);
                    String[] tmp2 = item2.Split(item_Split, StringSplitOptions.RemoveEmptyEntries);
                    String c = "";
                    if (tmp1.Length == 1)
                    {
                        if (tmp1[0].CompareTo(tmp2[0]) < 0)
                        {
                            c = tmp1[0] + itemSplit + tmp2[0] + itemSplit;
                        }
                    }
                    else
                    {
                        bool flag = true;
                        for (int i = 0; i < tmp1.Length - 1; i++)
                        {
                            if (tmp1[i].Equals(tmp2[i]) == false)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag == true && (tmp1[tmp1.Length - 1].CompareTo(tmp2[tmp2.Length - 1]) < 0))
                        {
                            c = item1 + tmp2[tmp2.Length - 1] + itemSplit;
                        }
                    }
                    bool Set = false;
                    if (c != "")
                    {
                        String[] tmpC = c.Split(item_Split, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < tmpC.Length; i++)
                        {
                            String subC = "";
                            for (int j = 0; j < tmpC.Length; j++)
                            {
                                if (i != j)
                                {
                                    subC = subC + tmpC[j] + itemSplit;
                                }
                            }
                            if (item.ContainsKey(subC) == false)
                            {
                                Set = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Set = true;
                    }
                    if (Set == false)
                    {
                        dateCollection.Add(c, 0);
                    }
                }
            }
            return dateCollection;
        }
        public Dictionary<String, int> get() 
        {
            Dictionary<String, int> Collections = new Dictionary<String, int>();
            foreach (KeyValuePair<string, int> item in getItem())
            {
                Collections.Add(item.Key, item.Value);
            }
            Dictionary<String, int> itemkFc = new Dictionary<String, int>();
            foreach (KeyValuePair<string, int> item in getItem())
            {
                itemkFc.Add(item.Key, item.Value);
            }
            while (itemkFc != null && itemkFc.Count != 0)
            {
                Dictionary<String, int> candidateCollection = getCollection(itemkFc);
                List<String> ccKeySet = candidateCollection.Keys.ToList();
              
                foreach (String trans in transList)
                {
                    foreach (String candidate in ccKeySet)
                    {
                        bool flag = true; 
                        String[] candidateItems = candidate.Split(item_Split, StringSplitOptions.RemoveEmptyEntries);
                        foreach (String candidateItem in candidateItems)
                        {
                            if (trans.IndexOf(candidateItem + itemSplit) == -1)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag == true)
                        {
                            int count = candidateCollection[candidate];
                            candidateCollection.Remove(candidate);
                            candidateCollection.Add(candidate, count + 1);
                        }
                    }
                }
                itemkFc.Clear();
                foreach (String candidate in ccKeySet)
                {
                    int count = candidateCollection[candidate];
                    if (count >= support)
                    {
                        itemkFc.Add(candidate, count);
                    }
                }
                foreach (KeyValuePair<string, int> item in itemkFc)
                {
                    if (Collections.ContainsKey(item.Key))
                    {
                        Collections.Remove(item.Key);
                    }
                    Collections.Add(item.Key, item.Value);
                }
            }
            return Collections;
        }
        private void build(List<String> sourceSet, List<List<String>> result) 
        {
            
            if (sourceSet.Count == 1)
            {
                List<String> set = new List<String>();
                set.Add(sourceSet[0]);
                result.Add(set);
            }
            else if (sourceSet.Count > 1)
            {
                build(sourceSet.Take(sourceSet.Count - 1).ToList(), result);
                int size = result.Count;
               
                List<String> single = new List<String>();
                single.Add(sourceSet[sourceSet.Count - 1]);
                result.Add(single);
               
                List<String> clone;
                for (int i = 0; i < size; i++)
                {
                    clone = new List<String>();
                    foreach (String str in result[i])
                    {
                        clone.Add(str);
                    }
                    clone.Add(sourceSet[sourceSet.Count - 1]);
                    result.Add(clone);
                }
            }
        }
        public Dictionary<String, Double> Rules(Dictionary<String, int> Collection) 
        {
            Dictionary<String, Double> Rule = new Dictionary<String, Double>();
            List<String> keySet = Collection.Keys.ToList();
            foreach (String key in keySet)
            {
                double countAll = Collection[key];
                String[] keyItems = key.Split(item_Split, StringSplitOptions.RemoveEmptyEntries);
                if (keyItems.Length > 1)
                {
                    List<String> source = keyItems.ToList();
                    List<List<String>> result = new List<List<String>>();
                    build(source, result); 
                    foreach (List<String> itemList in result)
                    {
                        if (itemList.Count < source.Count)
                        {   
                            List<String> otherList = new List<String>();
                            foreach (String sourceItem in source)
                            {
                                if (!itemList.Contains(sourceItem))
                                {
                                    otherList.Add(sourceItem);
                                }
                            }
                            String reasonStr = "";
                            String resultStr = "";
                            foreach (String item in itemList)
                            {
                                reasonStr = reasonStr + item + itemSplit;
                            }
                            foreach (String item in otherList)
                            {
                                resultStr = resultStr + item + itemSplit;
                            }
                            double countReason = Collection[reasonStr];
                            double itemConfidence = countAll / countReason;
                            if (itemConfidence >= confidence)
                            {
                                String rule = reasonStr + CON + resultStr;
                                Rule.Add(rule, itemConfidence);
                            }
                        }
                    }
                }
            }
            return Rule;
        }
    }
}
