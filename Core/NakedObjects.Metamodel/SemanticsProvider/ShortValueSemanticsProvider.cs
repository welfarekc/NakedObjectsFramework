// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Globalization;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.Spec;
using NakedObjects.Architecture.SpecImmutable;
using NakedObjects.Core;
using NakedObjects.Core.Util;

namespace NakedObjects.Meta.SemanticsProvider {
    [Serializable]
    public sealed class ShortValueSemanticsProvider : ValueSemanticsProviderAbstract<short>, IShortValueFacet {
        private const short DefaultValueConst = 0;
        private const bool EqualByContent = true;
        private const bool Immutable = true;
        private const int TypicalLengthConst = 6;

        public ShortValueSemanticsProvider(IObjectSpecImmutable spec, ISpecification holder)
            : base(Type, holder, AdaptedType, TypicalLengthConst, Immutable, EqualByContent, DefaultValueConst, spec) {}

        public static Type Type {
            get { return typeof (IShortValueFacet); }
        }

        public static Type AdaptedType {
            get { return typeof (short); }
        }

        #region IShortValueFacet Members

        public short ShortValue(INakedObjectAdapter nakedObjectAdapter) {
            return nakedObjectAdapter.GetDomainObject<short>();
        }

        #endregion

        public static bool IsAdaptedType(Type type) {
            return type == typeof (short);
        }

        protected override short DoParse(string entry) {
            try {
                return short.Parse(entry, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands);
            }
            catch (FormatException) {
                throw new InvalidEntryException(FormatMessage(entry));
            }
            catch (OverflowException) {
                throw new InvalidEntryException(OutOfRangeMessage(entry, short.MinValue, short.MaxValue));
            }
        }

        protected override short DoParseInvariant(string entry) {
            return short.Parse(entry, CultureInfo.InvariantCulture);
        }

        protected override string GetInvariantString(short obj) {
            return obj.ToString(CultureInfo.InvariantCulture);
        }

        protected override string TitleStringWithMask(string mask, short value) {
            return value.ToString(mask);
        }

        protected override string DoEncode(short obj) {
            return obj.ToString("G", CultureInfo.InvariantCulture);
        }

        protected override short DoRestore(string data) {
            return short.Parse(data, CultureInfo.InvariantCulture);
        }

        public override string ToString() {
            return "ShortAdapter: ";
        }
    }
}