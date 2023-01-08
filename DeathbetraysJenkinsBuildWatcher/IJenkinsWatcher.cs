using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeathbetraysJenkinsBuildWatcher
{
    public interface IJenkinsWatcher
    {
        JenkinsBuildChannel GetBuild(string _buildName);
        JenkinsBuildGroup GetBuildGroup(string _buildGroup);
    }
}
