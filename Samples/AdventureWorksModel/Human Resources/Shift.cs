// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.ComponentModel.DataAnnotations;
using NakedObjects;

namespace AdventureWorksModel {
    [Bounded]
    [IconName("clock.png")]
    public class Shift : AWDomainObject {
        #region ID

        [NakedObjectsIgnore]
        public virtual byte ShiftID { get; set; }

        #endregion

        #region Name

        [Title]
        [MemberOrder(1)]
        [StringLength(50)]
        [TypicalLength(10)]
        public virtual string Name { get; set; }

        #endregion

        #region ModifiedDate

        [MemberOrder(99)]
        [Disabled]
        public override DateTime ModifiedDate { get; set; }

        #endregion

        #region Complex Types

        private TimePeriod times = new TimePeriod();

        [MemberOrder(2)]
        public virtual TimePeriod Times {
            get { return times; }
            set { times = value; }
        }

        #endregion
    }
}