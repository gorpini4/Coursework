using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Windows;

namespace KursovaKomiv_3kurs
{
    public partial class Form1 : Form
    {
        AlgoStatistic[] answerList;
        public Form1()
        {
            InitializeComponent();
            answerList = new AlgoStatistic[COUNT*nameAlgo.Length + 1];
        }
        private int N = 5;
        private const int COUNT = 100;
        private int numIteration = 0;
        int[,] d;
        struct AlgoStatistic
        {
            public int type;
            public long time;
            public int len;
            public int[] path;
            public AlgoStatistic(int Type, long Time, int Len)
            {
                this.type = Type;
                this.time = Time;
                this.len = Len;
                this.path = null;
            }
            public AlgoStatistic(int Type, long Time, int Len, int[] Path)
            {
                this.type = Type;
                this.time = Time;
                this.len = Len;
                this.path = Path;
            }
        }
        int aLast = 0;
        /*
         * type:
         *          0 - Повний перебір
         *          1 - Перебір масок
         *          2 - Метод віток та границь
         *          3 - Жадний алгоритм
         *          4 - Метод імітації відпалу
         *          5 - Метод імітації відпалу
         *          6 - Метод імітації відпалу
         */
        private readonly string[] nameAlgo = {
            "Повний перебір",
            "Перебір масок",
            "Метод віток та границь",
            "Жадний алгоритм",
            " Метод імітації відпалу(400,0.001)",
            " Метод імітації відпалу(10,0.3)",
            " Метод імітації відпалу(10,0.003)"
                                             };
        Stopwatch swPereborMasok = new Stopwatch();
        Stopwatch swBruteForce = new Stopwatch();
        Stopwatch swSimulateAnnealing = new Stopwatch();
        Stopwatch swGreedy = new Stopwatch();
        Stopwatch swBranchBound = new Stopwatch();
        AlgoStatistic tempAlgoStat;


       
        private void butPerebor_Click(object sender, EventArgs e)
        {
            MessageBox.Show( AlgoStatToStr( tempAlgoStat =  BruteForceSearch(),true));
            visual(tempAlgoStat);
        }
        #region Перебір масок
        bool get(int nmb, Int64 x)
        { 
            return (x & (1 << nmb)) != 0; 
        }

        private AlgoStatistic PereborMasok()
        {
            swPereborMasok.Start();
            int inf = int.MaxValue / 2;
            int n, j, k, ans;
            int[,] t = new int[1 << N, N];
            Int64 temp, m,i;
            n = N;  
            //indexToMatrix(int.MaxValue / 2);
            t[1, 0] = 0;
            m = 1 << n;
            for (i = 1; i < m; i += 2)
            {
                for (j = (i == 1) ? 1 : 0; j < n; ++j)
                {

                    t[i, j] = inf;
                    if (j > 0 && get(j, i))
                    {
                        temp = i ^ (1 << j);
                        for (k = 0; k < n; ++k)
                            if (get(k, i) && d[k, j] > 0)
                                if (t[i, j] >= t[temp, k] + d[k, j])
                                    t[i, j] = t[temp, k] + d[k, j];
                    }
                }
            }
            for (j = 1, ans = inf; j < n; ++j)
                if (d[j, 0] > 0) ans = Math.Min(ans, t[m - 1, j] + d[j, 0]);
            swPereborMasok.Stop();
            return new AlgoStatistic(1, swPereborMasok.ElapsedMilliseconds, ans);
        }
        #endregion

        #region simulated anneling
        // Calculate the acceptance probability
        private static double acceptanceProbability(double delta, double temperature)
        {
            // If the new solution is worse, calculate an acceptance probability
            return Math.Exp(-(delta) / temperature);
        }
        private void createRandomPath(ref int[] mas)
        {
            int[] tempMas = new int[N];
            for (int i = 0; i < N; i++)
            {
                tempMas[i] = i;
            }
            tempMas = tempMas.OrderBy(n => Guid.NewGuid()).ToArray();
            for (int i = 0; i < N; i++)
            {
                mas[i] = tempMas[i];
            }
            mas[N] = mas[0];
        }
        private AlgoStatistic SimulatedAnnealing(double temperature, double coolingRate)
        {
            int type = 3;
            if (temperature == 10 && coolingRate == 0.3) type = 5;
            else
            if (temperature == 10 && coolingRate == 0.003) type = 6;
            else
            if (temperature == 400 && coolingRate == 0.001) 
                type = 4;

            swSimulateAnnealing.Start();
            int temp, i, sum = 0, randomStep = 0;
            int[] Path = new int[N + 1];
            int[] bestPath = new int[N + 1];
            int bestSum,sumNow,sumNew;
            Random random = new Random();
            createRandomPath(ref Path);
            Path = GreedyAlgorithm(random.Next(0,N-1)).path;
            //string ss = "randPath: "+Path[0].ToString();
            Array.Copy(Path, bestPath, N + 1);
            bestSum = 0;
            for (i = 0; i < N; i++)
            {
                //ss +=  "->"+Path[i + 1].ToString();
                bestSum += d[Path[i], Path[i + 1]];
            }
            //ss += "\n sum = " + bestSum.ToString() + "\n";
            sum = bestSum;
            //MessageBox.Show(ss);
            //richTBshortStory.AppendText(ss);
            while (temperature > 0.001)
            {
                //выбираем два случайных города
                //первый и последний индексы не трогаем
                int p1 = 0, p2 = 0;
                while (p1 == p2)
                {
                    p1 = random.Next(1, Path.Length - 1);
                    p2 = random.Next(1, Path.Length - 1);
                }
                //проверка расстояний
                sumNow = d[Path[p1 - 1], Path[p1]] + d[Path[p1], Path[p1 + 1]] +
                              d[Path[p2 - 1], Path[p2]] + d[Path[p2], Path[p2 + 1]];
                sumNew = d[Path[p1 - 1], Path[p2]] + d[Path[p2], Path[p1 + 1]] +
                              d[Path[p2 - 1], Path[p1]] + d[Path[p1], Path[p2 + 1]];  
                
                if (sumNew <= sumNow)
                {
                    temp = Path[p1];
                    Path[p1] = Path[p2];
                    Path[p2] = temp;
                    sum += sumNew - sumNow;
                  
                }
                else
                {
                    if (acceptanceProbability(sumNew - sumNow, temperature) > random.NextDouble())
                    {
                        sum += sumNew - sumNow;
                        temp = Path[p1];
                        Path[p1] = Path[p2];
                        Path[p2] = temp;
                       
                        randomStep++;
                    }
                }
                sum = 0;
                for (i = 0; i < N; i++)
                        {
                            sum += d[Path[i], Path[i + 1]];
                        }
                if (bestSum > sum)
                {
                    Array.Copy(Path, bestPath, N + 1);
                    bestSum=0;
                    for (i = 0; i < N; i++)
                    {
                        bestSum += d[bestPath[i], bestPath[i + 1]];
                    }
                    sum = 0;
                    for (i = 0; i < N; i++)
                    {
                        sum += d[Path[i], Path[i + 1]];
                    }

                 //   bestSum = sum;
                }
                // Cool system
                temperature *= 1 - coolingRate;
            }
            sum = 0;
            for (i = 0; i < N; i++)
            {
                sum += d[bestPath[i], bestPath[i + 1]];
            }
            swSimulateAnnealing.Stop();
            return new AlgoStatistic(type, swSimulateAnnealing.ElapsedMilliseconds, sum,bestPath);

        }
        #endregion

        #region Жадний алгоритм
        private AlgoStatistic GreedyAlgorithm(int start = 1) //жадний алгоритм для задачі Комівояжера
        {
            swGreedy.Start();
            int i, j, res = 0, min, tmp = 0;
            int[] ans = new int[N+1];
            bool[] visited = new bool[N];
            Array.Clear(visited, 0, N);
            //int start = Convert.ToInt32(numericW.Value);
            
            //indexToMatrix(int.MaxValue / 2);
            visited[start] = true;
            ans[0] = start;
            res++;
            tmp = start;
            for (i = 0; i < N - 1; i++)
            {
                min = -1;

                for (j = 0; j < N; j++)
                    if (!visited[j] && d[tmp, j] > 0)
                    {
                        if (min == -1)
                            min = j;
                        else
                            if (d[tmp, j] < d[tmp, min])
                                min = j;
                    }
                ans[res] = min;
                if (min < 0)
                {
                    MessageBox.Show("can't count");
                    return new AlgoStatistic(0, 0, 0);

                }
                visited[min] = true;
                tmp = min;
                res++;
            }
            ans[N] = ans[0];
            int sum = 0;
            for (i = 0; i < N ; i++)
            {
                sum += d[ans[i], ans[i + 1]];
            }
            swGreedy.Stop();
            return new AlgoStatistic(3, swGreedy.ElapsedMilliseconds, sum, ans);
        }
        #endregion

        #region BruteForceSearch

        int[] genMas;
        int[] answerBruteForceSearch;
        int sumAnswerBruteForceSearch;

        private void goCheck()
        {
            int sum = 0;
            genMas[N] = genMas[0];
            for (int i = 0; i < N; i++)
            {
                sum += d[genMas[i], genMas[i + 1]];
            }
            if (sum < sumAnswerBruteForceSearch)
            {
                sumAnswerBruteForceSearch = sum;
                /*int[] temp;
                temp = genMas;
                genMas = answerBruteForceSearch;
                answerBruteForceSearch = temp;
                */
                for (int i = 0; i <= N; i++)
                {
                    answerBruteForceSearch[i] = genMas[i];
                }
            }
        }

        private void gen(int pos, int val)
        {
            bool flag = true;
            for (int i = 0; i < pos && flag; i++)
            {
                if (genMas[i] == val) flag = false;
            }
            if (flag)
            {
                genMas[pos] = val;
                if (pos + 1 == N)
                {
                    goCheck();
                }
                else
                    for (int i = 0; i < N; i++)
                        gen(pos + 1, i);
            }
        }
        
        private AlgoStatistic BruteForceSearch()
        {
            swBruteForce.Start();
            genMas = new int[N + 1];
            answerBruteForceSearch = new int[N + 1];
            sumAnswerBruteForceSearch = int.MaxValue;
            for (int i = 0; i < N; i++)
            {
                gen(0, i);
            }
            swBruteForce.Stop();
            return new AlgoStatistic(0, swBruteForce.ElapsedMilliseconds, sumAnswerBruteForceSearch,answerBruteForceSearch);
        }

        #endregion

        #region Метод віток та границь

        private AlgoStatistic BranchBound()
        {
            int i;
            for (i = 0; i < N; i++)
                d[i, i] = 0;

            swBranchBound.Start();

            ROUTE = new int[N + 1];
            PTR = new int[N];
            PATH_WEIGHT = 0;
            for (i = 0; i < N; i++) ROUTE[i] = i;
            FITSP(0);
            TWOOPT();
            THREEOPT();
            int sum = 0;
            for (i = 0; i < N - 1; i++)
            {
                sum += d[ROUTE[i], ROUTE[i + 1]];
            }
            sum += d[ROUTE[N - 1], ROUTE[0]];

            swBranchBound.Stop();
            return new AlgoStatistic(2, swBranchBound.ElapsedMilliseconds*10, sum, ROUTE);
        }
        struct SWAPRECORD
        {
            public int X1, X2, Y1, Y2, Z1, Z2, GAIN;
            public bool CHOICE;
        };
        int[] ROUTE;
        int PATH_WEIGHT;
        int INDEX;
        int[] PTR;
        SWAPRECORD BESTSWAP, SWAP;

        void FITSP(int S)
        {
            int END1 = 0, END2 = 0, FARTHEST = 0, I, INDEX, INSCOST, MAXDIST, NEWCOST, NEXTINDEX;
            int[] CYCLE = new int[N];
            int[] DIST = new int[N];
            for (I = 0; I < N; I++) CYCLE[I] = 0;
            CYCLE[S] = S;
            for (I = 0; I < N; I++) DIST[I] = d[S, I];
            PATH_WEIGHT = 0;
            int J = 0;
            for (I = 0; I < N - 1; I++)
            {
                MAXDIST = -int.MaxValue / 2;
                for (J = 0; J < N; J++)
                    if (CYCLE[J] == 0)
                        if (DIST[J] > MAXDIST)
                        {
                            MAXDIST = DIST[J];
                            FARTHEST = J;
                        }

                INSCOST = int.MaxValue / 2; INDEX = S;
                for (J = 0; J <= I; J++)
                {
                    NEXTINDEX = CYCLE[INDEX];
                    NEWCOST = d[INDEX, FARTHEST] + d[FARTHEST, NEXTINDEX] - d[INDEX, NEXTINDEX];
                    if (NEWCOST < INSCOST)
                    {


                        INSCOST = NEWCOST;
                        END1 = INDEX; END2 = NEXTINDEX;
                    }
                    INDEX = NEXTINDEX;
                }
                CYCLE[FARTHEST] = END2; CYCLE[END1] = FARTHEST;
                PATH_WEIGHT = PATH_WEIGHT + INSCOST;
                for (J = 0; J < N; J++)
                    if (CYCLE[J] == 0)
                        if (d[FARTHEST, J] < DIST[J]) DIST[J] = d[FARTHEST, J];
            }
            INDEX = S;
            for (I = 0; I < N; I++)
            {
                ROUTE[I] = INDEX; INDEX = CYCLE[INDEX];
            }
        }

        void SWAPCHECK(ref SWAPRECORD SWAP)
        {
            int DELWEIGHT, MAX;


            SWAP.GAIN = 0;
            DELWEIGHT = d[SWAP.X1, SWAP.X2] + d[SWAP.Y1, SWAP.Y2] + d[SWAP.Z1, SWAP.Z2];
            MAX = DELWEIGHT - (d[SWAP.Y1, SWAP.X1] + d[SWAP.Z1, SWAP.X2] + d[SWAP.Z2, SWAP.Y2]);
            if (MAX > SWAP.GAIN)
            {
                SWAP.GAIN = MAX; SWAP.CHOICE = false;
            }
            MAX = DELWEIGHT - (d[SWAP.X1, SWAP.Y2] + d[SWAP.Z1, SWAP.X2] + d[SWAP.Y1, SWAP.Z2]);
            if (MAX > SWAP.GAIN)
            {
                SWAP.GAIN = MAX; SWAP.CHOICE = true;
            }
        }

        void REVERSE(int START, int FINISH)
        {
            int AHEAD, LAST, NEXT;
            if (START != FINISH)
            {
                LAST = START; NEXT = PTR[LAST];
                do
                {
                    AHEAD = PTR[NEXT]; PTR[NEXT] = LAST;
                    LAST = NEXT; NEXT = AHEAD;
                }
                while (LAST != FINISH);
            }
        }

        void THREEOPT()
        {
            int I, J, K;
            for (I = 0; I < N - 1; I++) { PTR[ROUTE[I]] = ROUTE[I + 1]; }
            PTR[ROUTE[N - 1]] = ROUTE[0];
            do
            {
                BESTSWAP.GAIN = 0;
                SWAP.X1 = 1;
                for (I = 0; I < N - 1; I++)
                {
                    SWAP.X2 = PTR[SWAP.X1]; SWAP.Y1 = SWAP.X2;
                    for (J = 1; J < N - 3; J++)
                    {
                        SWAP.Y2 = PTR[SWAP.Y1]; SWAP.Z1 = PTR[SWAP.Y2];
                        for (K = J + 1; K < N - 1; K++)
                        {
                            SWAP.Z2 = PTR[SWAP.Z1];
                            SWAPCHECK(ref SWAP);
                            if (SWAP.GAIN > BESTSWAP.GAIN) BESTSWAP = SWAP;
                            SWAP.Z1 = SWAP.Z2;
                        }
                        SWAP.Y1 = SWAP.Y2;
                    }
                    SWAP.X1 = SWAP.X2;
                }
                if (BESTSWAP.GAIN > 0)
                {
                    if (BESTSWAP.CHOICE == false)
                    {
                        REVERSE(BESTSWAP.Z2, BESTSWAP.X1);
                        PTR[BESTSWAP.Y1] = BESTSWAP.X1; PTR[BESTSWAP.Z2] = BESTSWAP.Y2;
                    }
                    else
                    {
                        PTR[BESTSWAP.X1] = BESTSWAP.Y2; PTR[BESTSWAP.Y1] = BESTSWAP.Z2;
                    }
                    PTR[BESTSWAP.Z1] = BESTSWAP.X2;
                }
            } while (BESTSWAP.GAIN != 0);
            INDEX = 0;
            for (I = 0; I < N; I++)
            {
                ROUTE[I] = INDEX; INDEX = PTR[INDEX];
            }
        }

        void TWOOPT()
        {

            int AHEAD, I, I1, I2, INDEX, J, J1, J2, LAST, LIMIT, MAX, MAX1, NEXT, S1 = 0, S2 = 0, T1 = 0, T2 = 0;
            int[] PTR = new int[N];

            for (I = 0; I < N - 1; I++) { PTR[ROUTE[I]] = ROUTE[I + 1]; }
            PTR[ROUTE[N - 1]] = ROUTE[0];
            do
            {
                MAX = 0; I1 = 0;
                for (I = 0; I < N - 2; I++)
                {
                    if (I == 0) LIMIT = N - 1;
                    else LIMIT = N;
                    I2 = PTR[I1]; J1 = PTR[I2];
                    for (J = I + 2; J < LIMIT; J++)
                    {
                        J2 = PTR[J1];
                        MAX1 = d[I1, I2] + d[J1, J2] - (d[I1, J1] + d[I2, J2]);
                        if (MAX1 > MAX)
                        {
                            S1 = I1; S2 = I2;
                            T1 = J1; T2 = J2;
                            MAX = MAX1;
                        }
                        J1 = J2;
                    }
                    I1 = I2;
                }
                if (MAX > 0)
                {
                    PTR[S1] = T1;
                    NEXT = S2; LAST = T2;
                    do
                    {
                        AHEAD = PTR[NEXT]; PTR[NEXT] = LAST;
                        LAST = NEXT; NEXT = AHEAD;
                    } while (NEXT != T2);
                    PATH_WEIGHT = PATH_WEIGHT - MAX;
                }
            } while (MAX > 0);
            INDEX = 0;
            for (I = 0; I < N; I++)
            {
                ROUTE[I] = INDEX; INDEX = PTR[INDEX];
            }
        }


        

        #endregion

        private void butSimulated_Click(object sender, EventArgs e)
        {
            MessageBox.Show( AlgoStatToStr(tempAlgoStat= SimulatedAnnealing(400,0.001)));
            visual(tempAlgoStat);
        }

        private void butGreedy_Click(object sender, EventArgs e)
        {
            MessageBox.Show(AlgoStatToStr(tempAlgoStat = GreedyAlgorithm()));
            visual(tempAlgoStat);
        }

        private void butPereborMasok_Click(object sender, EventArgs e)
        {
            MessageBox.Show(AlgoStatToStr(tempAlgoStat = PereborMasok()));
            visual(tempAlgoStat);
        }

        int[,] tempXY;
        private bool genereteMatrix()
        {
            tempXY= new int[2, N];
            Random rand = new Random();
            for (int i = 0; i < N; i++)
            {
                tempXY[0, i] = rand.Next(500);
                tempXY[1, i] = rand.Next(500);
            }
            for (int i = 0; i < N; i++)
            {
                for (int j = i; j < N; j++)
                {
                    d[i, j] = (int)Math.Sqrt((tempXY[0, i] - tempXY[0, j]) * (tempXY[0, i] - tempXY[0, j]) +
                                (tempXY[1, i] - tempXY[1, j]) * (tempXY[1, i] - tempXY[1, j]));
                    d[j, i] = d[i, j];
                    if (i != j && d[i, j] == 0) return false;
                }
                d[i, i] = int.MaxValue / 3;

            }
            return true;
        }

        private void butGen_Click(object sender, EventArgs e)
        {
            d = new int[N, N];
            FileOut = new StreamWriter(@"C:\kursovaTXT\genMatrix" + N.ToString() + ".txt");
            FileOut.WriteLine(COUNT.ToString());

            FileOut.WriteLine(COUNT.ToString());

            numIteration = 0;
            aLast = 0;
            timerDO.Interval = 100;
            timerDO.Enabled = true;
        }

        private Brush[] color= { Brushes.Red,Brushes.Blue,Brushes.Green,Brushes.Yellow,Brushes.Gray,Brushes.Pink,Brushes.White};

        private void visual(AlgoStatistic temp)
        {

           
            if (temp.path == null) return;
            Graphics g = pB.CreateGraphics();
            var font = new Font("Arial", 9);
            g.DrawString(nameAlgo[temp.type], font, color[temp.type], 10, temp.type*10);

            Pen p = new Pen(color[temp.type], 2);
            for (int i = 0; i < temp.path.Length-1; i++)
            {
                g.DrawLine(p, new Point(tempXY[0, temp.path[i]], tempXY[1, temp.path[i]]), new Point(tempXY[0, temp.path[i + 1]], tempXY[1, temp.path[i + 1]]));
            }
            g.DrawLine(p, new Point(tempXY[0, temp.path[0]], tempXY[1, temp.path[0]]),
                          new Point(tempXY[0, temp.path[temp.path.Length - 1]], tempXY[1, temp.path[temp.path.Length - 1]]));
        }

        private void butGen1xNxN_Click(object sender, EventArgs e)
        {
            //d = constmas;
            /*
            answerList[aLast] = PereborMasok();
            addToRTB();
            aLast++;
            answerList[aLast] = BruteForceSearch();
            addToRTB();
            aLast++;
            answerList[aLast] = GreedyAlgorithm();
            addToRTB();
            aLast++;*/
            if (!genereteMatrix()) MessageBox.Show("WOW");

            if (N < 24)
            {
                AlgoStatToStr(tempAlgoStat = PereborMasok());
                visual(tempAlgoStat);
            }
            if (N < 12)
            {
                AlgoStatToStr(answerList[aLast] = BruteForceSearch());
                visual(tempAlgoStat);
            }
            AlgoStatToStr(tempAlgoStat = BranchBound());
            visual(tempAlgoStat);
            AlgoStatToStr(tempAlgoStat = SimulatedAnnealing(10, 0.3));
            visual(tempAlgoStat);
            AlgoStatToStr(tempAlgoStat = SimulatedAnnealing(10, 0.003));
            visual(tempAlgoStat);
            AlgoStatToStr(tempAlgoStat = SimulatedAnnealing(400, 0.001));
            visual(tempAlgoStat);
            AlgoStatToStr(tempAlgoStat = GreedyAlgorithm());
            visual(tempAlgoStat);

            string s = "";
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    s += d[i, j] + ", ";
                }
                s += "\n";
            }
            richTBshortStory.AppendText(s + "\n");
        }

        private string normalizeString(string sName, long date)
        {
            sName = " ------------------------------------------------------------  " + sName;
            string tempString = date.ToString();
            while (tempString.Length < 10) tempString += "  ";
            return sName + tempString;
        }

        private string AlgoStatToStr(AlgoStatistic temp,bool toRTB = true,bool toFile=false)
        {
            /*
       * type:
       *          0 - Повний перебір
       *          1 - Перебір масок
       *          2 - Жадний алгоритм
       *          3 - Метод імітації відпалу (10;    0.3)
       *          4 - Метод імітації відпалу (10;  0.003)
       *          5 - Метод імітації відпалу (400; 0.001)
       */
            
            if (toRTB)
            {
                string s = " ---------------------------------------------- " + nameAlgo[temp.type] + ": \n";
                s += "Вартість: " + temp.len.ToString();
                s += " Час робботи: " + temp.time.ToString() + "\n";
                if (temp.path != null)
                {
                    for (int i = 0; i < N; i++)
                        s += temp.path[i].ToString() + "->";
                    s += temp.path[N] + "\n";
                }
                richTBshortStory.AppendText(s);
                return s;
            }
            if (toFile)
            {
                richTBshortStory.AppendText("+" + temp.type.ToString());
                FileOut.WriteLine(temp.type.ToString()+' ' + temp.len.ToString()+' ' +temp.time.ToString());
            }
            return "file";
        }
        
        private void timerDO_Tick(object sender, EventArgs e)
        {

            if ( !genereteMatrix()) return;
            timerDO.Enabled = false;
            numIteration++;
            if (N < 24)
                AlgoStatToStr( answerList[aLast] = PereborMasok(),false,true);
            else AlgoStatToStr(answerList[aLast] = new AlgoStatistic(1,0,0), false, true);
            aLast++;

            if (N < 12)
                AlgoStatToStr(answerList[aLast] = BruteForceSearch(), false, true);
            else AlgoStatToStr( answerList[aLast] = new AlgoStatistic(0, 0, 0),false,true);
            aLast++;
            AlgoStatToStr(answerList[aLast] = BranchBound(), false, true);
            /*if (answerList[aLast].len != answerList[aLast - 1].len)
            {
                string s = "\n";
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        s += d[i, j] + ", ";
                    }
                    s += "\n";
                }
                richTBshortStory.AppendText(s + "\n");
            }*/
            aLast++;
            AlgoStatToStr(answerList[aLast] = SimulatedAnnealing(10, 0.3), false, true);
            aLast++;
            AlgoStatToStr(answerList[aLast] = SimulatedAnnealing(10, 0.003), false, true);
            aLast++;
            AlgoStatToStr(answerList[aLast] = SimulatedAnnealing(400, 0.001), false, true);
            aLast++;
            AlgoStatToStr(answerList[aLast] = GreedyAlgorithm(), false, true);
            aLast++;
            if (COUNT == numIteration+1)
            {
                timerDO.Enabled = false;
                if (N == 165)
                {
                    return;
                }
                FileOut.Close();
                richTBshortStory.AppendText("\n");
                N++;
                butGen.PerformClick();
            }
            else
            {
                timerDO.Interval = 100;
                timerDO.Enabled = true;
            }
        }
        StreamWriter FileOut;

        int l = 30;
        const int minN = 101, maxN = 153;

            int countMetod=7;
        double[,] len = new double[maxN + 1, 7],
        time = new double[maxN + 1, 7];
        int[,] minlen = new int[maxN + 1, 7];
        double[] maxTime = new double[maxN + 1];

        
        private void butTestGO_Click(object sender, EventArgs e)
        {
            var font = new Font("Arial", 9);
            int[,] tempM = new int[countMetod,2];


            for (int k = minN; k <= maxN; k++)
            {
                countMetod = 7;
                int mincof=0;
                if (k < 12) mincof = 0;
                else if (k < 24) mincof = 1;
                else mincof = 2;

                StreamReader cin = new StreamReader(@"C:\kursovaTXT\genMatrix" + k.ToString() + ".txt");
                StreamWriter cout = new StreamWriter(@"C:\kursovaTXT\outStatistics" + k.ToString() + ".txt");
                int n = Convert.ToInt32(cin.ReadLine());
                for (int i = 0; i < n-1; i++)
                {
                    for (int j = 0; j < countMetod; j++)
                    {
                        double[] m = cin.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => double.Parse(s)).ToArray();
                        tempM[(int)m[0], 0] = (int)m[1];
                        tempM[(int)m[0], 1] = (int)m[2];
                    }
                    for(int j=4; j < countMetod; j++)
                    {
                        if (tempM[3, 0] == tempM[j, 0])
                            minlen[k,j]++;
                    }
                    for (int j = mincof; j < countMetod; j++)
                    {
                        len[k,j] += Math.Abs(tempM[j, 0]-tempM[2,0])*100/tempM[2,0];
                        time[k,j] += tempM[j, 1];
                    }
                    len[k, 0] = 0;
                    len[k, 1] = 0;

                }
                string st=k.ToString() + " - кількіть вершин\n";/* = k.ToString()+"\nсередня похибка(sum):\n";
                cout.WriteLine("середня похибка:");

                for (int i = mincof; i < countMetod; i++)
                {
                    cout.WriteLine(len[k,i]);
                    st += nameAlgo[i] + ' ' + len[k,i].ToString() + '\n';
                }
                cout.WriteLine();*/
                for (int j = mincof; j < countMetod; j++)
                {
                    time[k, j] = time[k, j] / n;
                    if (time[k, j] > maxTime[k]) maxTime[k] = time[k, j];
                    len[k,j] = (double)len[k,j]/(double)n;
                }

                st += "середня похибка:\n";
                cout.WriteLine("середня похибка:");

                for (int i = mincof; i < countMetod; i++)
                {
                    cout.WriteLine(len[k, i]);
                    st += nameAlgo[i] + ' ' + len[k, i].ToString() + '\n';
                }
                cout.WriteLine(); 
                st += "середній час:\n";
                cout.WriteLine("середній час:");

                for (int i = mincof; i < countMetod; i++)
                {
                    cout.WriteLine(len[k, i]);
                    st += nameAlgo[i] + ' ' + time[k, i].ToString() + '\n';
                }
                cout.WriteLine();
                /*st += "\nкількіть правильних відповідей:\n";
                cout.WriteLine("кількіть правильних відповідей:");
                for (int i = mincof; i < countMetod; i++)
                {
                    cout.WriteLine(len[k,i] );
                    st += nameAlgo[i] + ' ' + minlen[k,i].ToString() + '\n';
                }
                cout.WriteLine();*/
                
                st += '\n';
                richTBshortStory.AppendText(st);
                cout.Close();
                cin.Close();


            }
          


            Graphics g = pB.CreateGraphics();
            Pen p = new Pen(Brushes.Black, 2);
            g.DrawLine(p, l, 500 - l, l, l);
            g.DrawLine(p, l, 500 - l, 500 - l, 500 - l);
            for (int i = minN; i <= maxN; i += (maxN - minN) / Math.Min(10, maxN - minN))
            {
                g.DrawString(i.ToString(), font, Brushes.Black, (500 - 2 * l - l) /  (maxN - minN) * (i - minN) + l + l / 2, 500 - l);
            }
            for (int i = 10; i <= 100; i += 10)
            {
                g.DrawString(i.ToString(), font, Brushes.Black, 0, 500 - l - (500 - 2 * l) / 10 * i / 10);
            }
            StreamWriter Out = new StreamWriter(@"C:\kursovaTXT\ans.txt");
            string ss="";
            for(int j=2; j< 7;j++)
            {
                Out.WriteLine(nameAlgo[j]);
                ss+=nameAlgo[j]+'\n';
                for (int i = minN; i < maxN;i++ )
                {
                    Out.WriteLine("    "+i.ToString()+": "+ ((time[i, j] * 100 / maxTime[i])).ToString() );
                    ss+= "    "+i.ToString()+": "+ ((time[i, j] * 100 / maxTime[i])).ToString()  +'\n';
                }
            }
            Out.WriteLine(nameAlgo[0]);
            ss += nameAlgo[0] + '\n';
            for (int i = minN; i < 12; i++)
            {
                Out.WriteLine("    " + i.ToString() + ": " + ((time[i, 0] * 100 / maxTime[i])).ToString());
                ss += "    " + i.ToString() + ": " + ((time[i, 0] * 100 / maxTime[i])).ToString() + '\n';
            }
            richTBshortStory.AppendText(ss);
            Out.Close();

          

            drawLen(ref g, Brushes.Green, 3, 3);
            drawLen(ref g, Brushes.Gray, 3, 4);
            drawLen(ref g, Brushes.Pink, 3, 5);
            drawLen(ref g, Brushes.DarkBlue, 3, 6);

            //drawTime(ref g, Brushes.Aqua, 5, 0, 12);// повний перебір
           // drawTime(ref g, Brushes.Blue, 4, 1, 24);//перебір масок
            drawTime(ref g, Brushes.Yellow, 3, 2);//віток та границь

            drawTime(ref g, Brushes.Purple, 5, 3);
            drawTime(ref g, Brushes.Aqua, 4, 4);
            drawTime(ref g, Brushes.Blue, 3, 5);
            drawTime(ref g, Brushes.Red, 3, 6);
           


        }
        void drawTime(ref Graphics g, Brush color, int w, int num)
        {
            var font = new Font("Arial", 9);
            for (int i = minN; i < maxN; i++)
            {
                g.DrawLine(new Pen(color, w), (500 - 2 * l - l) / (maxN - minN) * (i - minN) + l + l / 2, (int)(500 - l - (500 - 2 * l) * (time[i, num]  / maxTime[i])),

                       (500 - 2 * l - l) / (maxN - minN) * (i + 1 - minN) + l + l / 2, (int)(500 - l - (500-2*l)*((time[i + 1, num]  / maxTime[i + 1]))));
            }
            g.DrawString(nameAlgo[num], font, color, 300, l+num * 10);
        }
        void drawTime(ref Graphics g, Brush color, int w, int num,int Limit)
        {
            var font = new Font("Arial", 9);
            for (int i = minN; i < maxN && i < Limit; i++)
            {
                g.DrawLine(new Pen(color, w), (500 - 2 * l - l) / (maxN - minN) * (i - minN) + l + l / 2, 500 - l - (int)((time[i, num] * 100 / maxTime[i])),

                       (500 - 2 * l - l) / (maxN - minN) * (i + 1 - minN) + l + l / 2, 500 - l - (int)((time[i + 1, num] * 100 / maxTime[i + 1])));
            }

            g.DrawString(nameAlgo[num], font, color, 300, l+num * 10);
        }
        void drawLen(ref Graphics g, Brush color, int w, int num)
        {
            var font = new Font("Arial", 9);
            for (int i = minN; i < maxN; i++)
            {
                g.DrawLine(new Pen(color, w), (500 - 2 * l - l) / (maxN - minN) * (i - minN) + l + l / 2, 500 - l - (int)((500-2*l)*len[i, num]/100),

                    (500 - 2 * l - l) / (maxN - minN) * (i + 1 - minN) + l + l / 2, 500 - l - (int)((500 - 2 * l) * len[i+1, num] / 100));
            }

            g.DrawString(nameAlgo[num], font, color, 40, l+num * 10);

        }
        void drawLen(ref Graphics g, Brush color, int w, int num,int Limit)
        {
            var font = new Font("Arial", 9);
            for (int i = minN; i < maxN; i++)
            {
                g.DrawLine(new Pen(color, w), (500 - 2 * l - l) / (maxN - minN) * (i - minN) + l + l / 2, 500 - l - (int)((500 - 2 * l) * len[i, num] / 100),

                    (500 - 2 * l - l) / (maxN - minN) * (i + 1 - minN) + l + l / 2, 500 - l - (int)((500 - 2 * l) * len[i + 1, num] / 100));
            }

            g.DrawString(nameAlgo[num], font, color, 40,l+ num * 10);
        }
        int[,] constmas = {
                             {0, 188, 80, 321, 468}, 
                             {188, 0, 147, 157, 288}, 
                             {80, 147, 0, 300, 404}, 
                             {321, 157, 300, 0, 265}, 
                             {468, 288, 404, 265, 0}, 
                          };


        private void butBranchBound_Click(object sender, EventArgs e)
        {
            MessageBox.Show( AlgoStatToStr( tempAlgoStat = BranchBound()));
            visual(tempAlgoStat);
        }

        private void pB_Click(object sender, EventArgs e)
        {
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            N = Convert.ToInt32(numericN.Value);
            d = new int[N, N];
            genereteMatrix();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            N = Convert.ToInt32(numericN.Value);
            d = new int[N, N];
            genereteMatrix();
        }

        private void buttonConstMas_Click(object sender, EventArgs e)
        {
            numericN.Value = (int)Math.Sqrt(d.Length); 
            d = constmas;
            /*AlgoStatToStr(tempAlgoStat = SimulatedAnnealing(400, 0.001));
            visual(tempAlgoStat);
            AlgoStatToStr(tempAlgoStat = GreedyAlgorithm());
            visual(tempAlgoStat);*/
            AlgoStatToStr(tempAlgoStat = BranchBound());
            visual(tempAlgoStat);
            AlgoStatToStr(tempAlgoStat = PereborMasok());
            visual(tempAlgoStat);
            AlgoStatToStr(tempAlgoStat = BruteForceSearch());
            visual(tempAlgoStat);
        }
        private void recreateGen()
        {
            for (int i = 101; i < 154; i++)
            {
                StreamReader cin = new StreamReader(@"C:\kursovaTXT\genMatrix" + i.ToString() + ".txt");
                StreamWriter cout = new StreamWriter(@"C:\kursovaTXT2\genMatrix" + i.ToString() + ".txt");
                int n = Convert.ToInt32( cin.ReadLine());
                cout.WriteLine(n.ToString());
                int temp = 7;
                if (i < 13) temp = 7;
                else
                if (i < 24) temp = 6;
                else temp = 5;
                for (int k = 0; k <= 832; k++)
                {
                    double[] m = cin.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => double.Parse(s)).ToArray();
                    if (m[0] == 6) m[0] = 2;
                    else                    if (m[0] == 2) m[0] = 3;
                        else     if (m[0] == 3) m[0] = 6;
                    cout.WriteLine(m[0].ToString() + ' ' + m[1].ToString() + ' ' + m[2].ToString());
                }
                cout.Close();
                cin.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            

        }
    }
}
