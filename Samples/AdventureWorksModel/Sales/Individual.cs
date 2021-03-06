// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.ComponentModel;
using NakedObjects;

namespace AdventureWorksModel {
    [IconName("person.png")]
    public class Individual : Customer {

        #region Life Cycle Methods
        public override void Persisting() {
            base.Persisting();
            ModifiedDate = DateTime.Now;
        }

        public override void Updating() {
            base.Updating();
            ModifiedDate = DateTime.Now;
        }
        #endregion

        #region Contact

        [NakedObjectsIgnore]
        public virtual int ContactID { get; set; }

        [MemberOrder(20)]
        [DisplayName("Name & Contact Details")]
        public virtual Contact Contact { get; set; }

        #endregion

        #region ModifiedDate

        [MemberOrder(99)]
        [Disabled]
        public virtual DateTime ModifiedDate { get; set; }

        #endregion

        public override string ToString() {
            var t = Container.NewTitleBuilder();
            t.Append(Contact).Append(",", AccountNumber);
            return t.ToString();
        }

        #region Demographics

        [NakedObjectsIgnore]
        public virtual string Demographics { get; set; }

        [DisplayName("Demographics")]
        [MemberOrder(30)]
        [MultiLine(NumberOfLines = 10)]
        [TypicalLength(500)]
        public virtual string FormattedDemographics {
            get { return Utilities.FormatXML(Demographics); }
        }

        #endregion
    }
}