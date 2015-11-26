using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

namespace OcrTask
{
    [TestFixture]
    public class Solution
    {
        [Test]
        public void FindSolution()
        {
            // �� ��� ��������� OneNote2013:
            var chunks = new[] { 111410, 341917, 137859, 113920, 119827, 266241, 160397, 512761, 101522, 186211, 484741, 772834, 781347, 473633, 535009, 921713, 183358, 553933, 172883, 162839, 100596, 193372, 135023, 100102, 594017, 314908, 137820, 631989, 318188, 535193, 449865, 109065, 771288, 112489, 172121, 136240, 157843, 163956, 192449, 114467, 189369, 989983, 233773, 194102, 159592, 108460, 146484, 680203, 610662, 231601, 203711, 148638, 109458, 183115, 592895, 437058, 189600, 194466, 582587, 194548, 886543, 199610, 153574, 103681, 170805, 904959, 604197, 276558, 580140, 105004, 931407, 153431, 856152, 698504, 874531, 192780, 202709, 220101, 169099, 120159, 235744, 119364, 117852, 206279, 116243, 225340, 142729, 137909, 883960, 188613, 854975, 132099, 345823, 160736, 856880, 977750, 167645, 636614, 204505, 898228, 157849, 169289, 125505, 850470, 204287, 550100, 134812, 115886, 133608, 166766, 130686, 179950, 803727, 191415, 119332, 948348, 124739, 145574, 147973, 147454, 115570, 101408, 149971, 113995, 837238, 162286, 192773, 102534, 384096, 733051, 152603, 814296, 188524, 355787, 940840, 168355, 649190, 500507, 648007, 148362, 195234, 346618, 106223, 847820, 171864, 120591, 184682, 696370, 790680, 148152, 213321, 307636, 130956, 659392, 164283, 171196, 306780, 177710, 517587, 144801, 377385, 173885, 192647, 889169, 148838, 180462, 194804, 208854, 107520, 148903, 134762, 205682, 108560, 935446, 167852, 818096, 170559, 107442, 222055, 107165, 746684, 209599, 341405, 127631, 114392, 199113, 130116, 166057, 142722, 377337, 104795, 928191, 117941, 189954, 103742, 784623, 188241, 149887, 210030, 185731, 131396, 149789, 164470, 118454, 996297, 135246, 667747, 899133, 555058, 178782, 149014, 110413, 394364, 528649, 420392, 725041, 198569, 191140, 365722, 701898, 117603, 100393, 468046, 653245, 991023, 129580, 230800, 161612, 185802, 822295, 108251, 196682, 211485, 855586, 184286, 872061, 147941, 166441, 450164, 179468, 199220, 236956, 196597, 916584, 589345, 238892, 359300, 464187, 194977, 170775, 171739, 121085, 169754, 143096, 214235, 903245, 183244, 777946, 983276, 943813, 192152, 367164, 209826, 110352, 111605, 764148, 271255, 151425, 242752, 533411, 200064, 115023, 138975, 636093, 188860, 168510, 127207, 494241, 369574, 533831, 169091, 176687, 135669, 194131, 169897, 423759, 100490, 121100, 262640, 479947 };
            foreach (var c1 in chunks)
                foreach (var c2 in chunks)
                    foreach (var c3 in chunks)
                    {
                        var s = "mars0_" + c1 + c2 + c3;
                        var hash = new MD5Cng().ComputeHash(Encoding.ASCII.GetBytes(s)).Select(b => b.ToString("x2")).Aggregate("", (ss, c) => ss + c);
                        if (hash.StartsWith("73f48ba"))
                            Console.WriteLine(s + " " + hash);
                    }
        }
    }
}