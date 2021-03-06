// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using NakedObjects;

namespace Expenses {
    namespace ExpenseClaims.Items {
        public class Hotel : AbstractExpenseItem {
            #region Hotel URL

            private string m_hotelURL;

            [MemberOrder(Sequence = "2.1"), Optionally]
            public virtual string HotelURL {
                get { return m_hotelURL; }

                set { m_hotelURL = value; }
            }

            public virtual void ModifyHotelURL(string newHotelURL) {
                HotelURL = newHotelURL;
                CheckIfCompleteAndRecalculateClaimTotalIfPersistent();
            }

            public virtual string DisableHotelURL() {
                return DisabledIfLocked();
            }

            #endregion

            #region Number of Nights

            private int m_numberOfNights;

            [MemberOrder(Sequence = "2.2")]
            public virtual int NumberOfNights {
                get { return m_numberOfNights; }

                set { m_numberOfNights = value; }
            }

            public virtual void ModifyNumberOfNights(int newNumberOfNights) {
                NumberOfNights = newNumberOfNights;
                CheckIfCompleteAndRecalculateClaimTotalIfPersistent();
            }

            public virtual string DisableNumberOfNights() {
                return DisabledIfLocked();
            }

            #endregion

            #region Accommodation

#pragma warning disable 612,618
            private decimal m_accommodation;

            [MemberOrder(Sequence = "2.3")]
            public virtual decimal Accommodation {
                get { return m_accommodation; }

                set { m_accommodation = value; }
            }

            public virtual void ModifyAccommodation(decimal newAccommodation) {
                Accommodation = newAccommodation;
                CheckIfComplete();
                RecalculateAmount();
            }

            public virtual string DisableAccommodation() {
                return DisabledIfLocked();
            }

            public virtual string ValidateAccommodation(decimal newAmount) {
                return ValidateAnyAmountField(newAmount);
            }

            #endregion

            #region Food

            private decimal m_food;

            [MemberOrder(Sequence = "2.4"), Optionally]
            public virtual decimal Food {
                get { return m_food; }

                set { m_food = value; }
            }

            public virtual void ModifyFood(decimal newMeals) {
                Food = newMeals;
                CheckIfComplete();
                RecalculateAmount();
            }

            public virtual string DisableFood() {
                return DisabledIfLocked();
            }

            public virtual string ValidateFood(decimal newAmount) {
                return ValidateAnyAmountField(newAmount);
            }

            #endregion

            #region Other

            private decimal m_other;

            [MemberOrder(Sequence = "2.5"), Optionally]
            public virtual decimal Other {
                get { return m_other; }

                set { m_other = value; }
            }

            public virtual void ModifyOther(decimal newOther) {
                Other = newOther;
                CheckIfComplete();
                RecalculateAmount();
            }

            public virtual string DisableOther() {
                return DisabledIfLocked();
            }

            public virtual string ValidateOther(decimal newAmount) {
                return ValidateAnyAmountField(newAmount);
            }

            #endregion

            #region Amount

            [Disabled]
            public override decimal Amount {
                get { return base.Amount; }
                set { base.Amount = value; }
            }

            [Hidden(WhenTo.Always)]
            public override void InitialiseAmount() {
                var zero = 0M;
                Accommodation = zero;
                Food = zero;
                Other = zero;
                Amount = (zero);
            }

            private void RecalculateAmount() {
                var newAmount = 0M;
                for (int i = 0; i < m_numberOfNights; i++) {
                    newAmount = AddIfNotNull(m_accommodation, newAmount);
                }
                newAmount = AddIfNotNull(m_food, newAmount);
                newAmount = AddIfNotNull(m_other, newAmount);
                ModifyAmount(newAmount);
            }

            private static decimal AddIfNotNull(decimal amountToAdd, decimal sum) {
                return sum + amountToAdd;
            }

            #endregion

            #region Copying

            protected internal override void CopyAllSameClassFields(AbstractExpenseItem otherItem) {
                base.CopyAllSameClassFields(otherItem);

                if (otherItem is Hotel) { }
            }

            protected internal override void CopyAnyEmptyFieldsSpecificToSubclassOfAbstractExpenseItem(AbstractExpenseItem otherItem) {
                if (otherItem is Hotel) {
                    var otherHotel = (Hotel) otherItem;
                    if (string.IsNullOrEmpty(m_hotelURL)) {
                        ModifyHotelURL(otherHotel.HotelURL);
                    }
                    if (m_numberOfNights == 0) {
                        ModifyNumberOfNights(otherHotel.NumberOfNights);
                    }
                    if (m_accommodation == 0) {
                        ModifyAccommodation(otherHotel.Accommodation);
                    }
                    if (m_food == 0) {
                        ModifyFood(otherHotel.Food);
                    }
                    if (m_other == 0) {
                        ModifyOther(otherHotel.Other);
                    }
                }
            }

            #endregion

            protected internal override bool MandatorySubClassFieldsComplete() {
                return m_hotelURL != null && !(m_hotelURL.Equals("")) && m_accommodation > 0M;
            }
#pragma warning restore 612,618
        }
    }
} //end of root namespace