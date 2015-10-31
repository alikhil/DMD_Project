using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Project_DMD.Models;

namespace Project_DMD.Classes
{
    public sealed class Global
    {
        static readonly Global _instance = new Global();
        public static readonly int ArticlePerPage = 30;
        public static Global Instance
        {
            get
            {
                return _instance;
            }
        } 

        public FakeDataRepository ArticlesRepository { get; set; }
        public IAppUserRepository UsersRepository { get; set; }
        public UserManager<AppUser> UserManager { get; set; }
        public Dictionary<string, string> Categories { get; set; }
 
        private Global()
        {
            ArticlesRepository = new FakeDataRepository();
            UsersRepository = new AppUserRepository();
            UserManager = new UserManager<AppUser>(new CustomUserStore());
            Categories = InitCategories();
        }

        public Dictionary<string, string> InitCategories()
        {
            #region arrays
            var shorts = new string[]
            {
                "stat.AP", "stat.CO", "stat.ML", "stat.ME", "stat.TH", "q-bio.BM", "q-bio.CB", "q-bio.GN", "q-bio.MN",
                "q-bio.NC", "q-bio.OT", "q-bio.PE", "q-bio.QM", "q-bio.SC", "q-bio.TO", "cs.AR", "cs.AI", "cs.CL",
                "cs.CC", "cs.CE", "cs.CG", "cs.GT", "cs.CV", "cs.CY", "cs.CR", "cs.DS", "cs.DB", "cs.DL", "cs.DM",
                "cs.DC", "cs.GL", "cs.GR", "cs.HC", "cs.IR", "cs.IT", "cs.LG", "cs.LO", "cs.MS", "cs.MA", "cs.MM",
                "cs.NI", "cs.NE", "cs.NA", "cs.OS", "cs.OH", "cs.PF", "cs.PL", "cs.RO", "cs.SE", "cs.SD", "cs.SC",
                "nlin.AO", "nlin.CG", "nlin.CD", "nlin.SI", "nlin.PS", "math.AG", "math.AT", "math.AP", "math.CT",
                "math.CA", "math.CO", "math.AC", "math.CV", "math.DG", "math.DS", "math.FA", "math.GM", "math.GN",
                "math.GT", "math.GR", "math.HO", "math.IT", "math.KT", "math.LO", "math.MP", "math.MG", "math.NT",
                "math.NA", "math.OA", "math.OC", "math.PR", "math.QA", "math.RT", "math.RA", "math.SP", "math.ST",
                "math.SG", "astro-ph", "cond-mat.dis-nn", "cond-mat.mes-hall", "cond-mat.mtrl-sci", "cond-mat.other",
                "cond-mat.soft", "cond-mat.stat-mech", "cond-mat.str-el", "cond-mat.supr-con", "gr-qc", "hep-ex",
                "hep-lat", "hep-ph", "hep-th", "math-ph", "nucl-ex", "nucl-th", "physics.acc-ph", "physics.ao-ph",
                "physics.atom-ph", "physics.atm-clus", "physics.bio-ph", "physics.chem-ph", "physics.class-ph",
                "physics.comp-ph", "physics.data-an", "physics.flu-dyn", "physics.gen-ph", "physics.geo-ph",
                "physics.hist-ph", "physics.ins-det", "physics.med-ph", "physics.optics", "physics.ed-ph",
                "physics.soc-ph", "physics.plasm-ph", "physics.pop-ph", "physics.space-ph", "quant-ph"
            };
            var longs = new string[]
            {
                "Statistics - Applications", "Statistics - Computation", "Statistics - Machine Learning",
                "Statistics - Methodology", "Statistics - Theory", "Quantitative Biology - Biomolecules",
                "Quantitative Biology - Cell Behavior", "Quantitative Biology - Genomics",
                "Quantitative Biology - Molecular Networks", "Quantitative Biology - Neurons and Cognition",
                "Quantitative Biology - Other", "Quantitative Biology - Populations and Evolution",
                "Quantitative Biology - Quantitative Methods", "Quantitative Biology - Subcellular Processes",
                "Quantitative Biology - Tissues and Organs", "Computer Science - Architecture",
                "Computer Science - Artificial Intelligence", "Computer Science - Computation and Language",
                "Computer Science - Computational Complexity",
                "Computer Science - Computational Engineering; Finance; and Science",
                "Computer Science - Computational Geometry", "Computer Science - Computer Science and Game Theory",
                "Computer Science - Computer Vision and Pattern Recognition", "Computer Science - Computers and Society",
                "Computer Science - Cryptography and Security", "Computer Science - Data Structures and Algorithms",
                "Computer Science - Databases", "Computer Science - Digital Libraries",
                "Computer Science - Discrete Mathematics",
                "Computer Science - Distributed; Parallel; and Cluster Computing",
                "Computer Science - General Literature", "Computer Science - Graphics",
                "Computer Science - Human-Computer Interaction", "Computer Science - Information Retrieval",
                "Computer Science - Information Theory", "Computer Science - Learning",
                "Computer Science - Logic in Computer Science", "Computer Science - Mathematical Software",
                "Computer Science - Multiagent Systems", "Computer Science - Multimedia",
                "Computer Science - Networking and Internet Architecture",
                "Computer Science - Neural and Evolutionary Computing", "Computer Science - Numerical Analysis",
                "Computer Science - Operating Systems", "Computer Science - Other", "Computer Science - Performance",
                "Computer Science - Programming Languages", "Computer Science - Robotics",
                "Computer Science - Software Engineering", "Computer Science - Sound",
                "Computer Science - Symbolic Computation", "Nonlinear Sciences - Adaptation and Self-Organizing Systems",
                "Nonlinear Sciences - Cellular Automata and Lattice Gases", "Nonlinear Sciences - Chaotic Dynamics",
                "Nonlinear Sciences - Exactly Solvable and Integrable Systems",
                "Nonlinear Sciences - Pattern Formation and Solitons", "Mathematics - Algebraic Geometry",
                "Mathematics - Algebraic Topology", "Mathematics - Analysis of PDEs", "Mathematics - Category Theory",
                "Mathematics - Classical Analysis and ODEs", "Mathematics - Combinatorics",
                "Mathematics - Commutative Algebra", "Mathematics - Complex Variables",
                "Mathematics - Differential Geometry", "Mathematics - Dynamical Systems",
                "Mathematics - Functional Analysis", "Mathematics - General Mathematics",
                "Mathematics - General Topology", "Mathematics - Geometric Topology", "Mathematics - Group Theory",
                "Mathematics - History and Overview", "Mathematics - Information Theory",
                "Mathematics - K-Theory and Homology", "Mathematics - Logic", "Mathematics - Mathematical Physics",
                "Mathematics - Metric Geometry", "Mathematics - Number Theory", "Mathematics - Numerical Analysis",
                "Mathematics - Operator Algebras", "Mathematics - Optimization and Control", "Mathematics - Probability",
                "Mathematics - Quantum Algebra", "Mathematics - Representation Theory",
                "Mathematics - Rings and Algebras", "Mathematics - Spectral Theory", "Mathematics - Statistics",
                "Mathematics - Symplectic Geometry", "Astrophysics", "Physics - Disordered Systems and Neural Networks",
                "Physics - Mesoscopic Systems and Quantum Hall Effect", "Physics - Materials Science", "Physics - Other",
                "Physics - Soft Condensed Matter", "Physics - Statistical Mechanics",
                "Physics - Strongly Correlated Electrons", "Physics - Superconductivity",
                "General Relativity and Quantum Cosmology", "High Energy Physics - Experiment",
                "High Energy Physics - Lattice", "High Energy Physics - Phenomenology", "High Energy Physics - Theory",
                "Mathematical Physics", "Nuclear Experiment", "Nuclear Theory", "Physics - Accelerator Physics",
                "Physics - Atmospheric and Oceanic Physics", "Physics - Atomic Physics",
                "Physics - Atomic and Molecular Clusters", "Physics - Biological Physics", "Physics - Chemical Physics",
                "Physics - Classical Physics", "Physics - Computational Physics",
                "Physics - Data Analysis; Statistics and Probability", "Physics - Fluid Dynamics",
                "Physics - General Physics", "Physics - Geophysics", "Physics - History of Physics",
                "Physics - Instrumentation and Detectors", "Physics - Medical Physics", "Physics - Optics",
                "Physics - Physics Education", "Physics - Physics and Society", "Physics - Plasma Physics",
                "Physics - Popular Physics", "Physics - Space Physics", "Quantum Physics"
            };
#endregion
            return  longs.Zip(shorts, (s, i) => new { s, i })
                          .ToDictionary(item => item.i, item => item.s);
        }
    }
}