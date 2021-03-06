// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Collections.Generic;
using NakedObjects;

namespace Expenses.RecordedActions {
    /// <summary>
    ///     Defines user actions made available on objects implementing RecordedActionContext
    /// </summary>
    [Named("Recorded Actions")]
    public class RecordedActionContributedActions {
        public IDomainObjectContainer Container { protected get; set; }

        public virtual IList<RecordedAction> AllRecordedActions([ContributedAction(SubMenu = "Recorded Actions", Id = "Claim-RecordedActionContributedActions:")] IRecordedActionContext context) {
            return m_recordedActionRepository.allRecordedActions(context);
        }

        /// <summary>
        /// This is to test the effect (on menus) of a contributed action that has been hidden (e.g. by authorization)
        /// </summary>
        /// <param name="context"></param>
#pragma warning disable 618
        [Hidden]
#pragma warning restore 618
        public void HiddenContributedAction([ContributedAction(SubMenu = "Recorded Actions", Id = "Claim-RecordedActionContributedActions:")] IRecordedActionContext context) {

        }

        #region Injected Services

        #region Injected: RecordedActionRepository

        private RecordedActionRepository m_recordedActionRepository;

        public RecordedActionRepository RecordedActionRepository {
            set { m_recordedActionRepository = value; }
        }

        #endregion

        //
        //		* This region contains references to the services (Repositories, 
        //		* Factories or other Services) used by this domain object.  The 
        //		* references are injected by the application container.
        //		

        #endregion
    }
}