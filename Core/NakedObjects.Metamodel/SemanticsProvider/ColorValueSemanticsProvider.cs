// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Drawing;
using System.Globalization;
using NakedObjects.Architecture;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.Spec;
using NakedObjects.Architecture.SpecImmutable;
using NakedObjects.Capabilities;

namespace NakedObjects.Meta.SemanticsProvider {
    public class ColorValueSemanticsProvider : ValueSemanticsProviderAbstract<Color>, IPropertyDefaultFacet {
        private const bool EqualByContent = true;
        private const bool Immutable = true;
        private const int TypicalLengthConst = 4;
        private static readonly Color DefaultValueConst = Color.Black;

        /// <summary>
        ///     Required because implementation of <see cref="IParser{T}" /> and <see cref="IEncoderDecoder{T}" />.
        /// </summary>
        public ColorValueSemanticsProvider(IObjectSpecImmutable spec)
            : this(spec, null) {}

        public ColorValueSemanticsProvider(IObjectSpecImmutable spec, ISpecification holder)
            : base(Type, holder, AdaptedType, TypicalLengthConst, Immutable, EqualByContent, DefaultValueConst, spec) {}

        public static Type Type {
            get { return typeof (IColorValueFacet); }
        }

        public static Type AdaptedType {
            get { return typeof (Color); }
        }

        #region IPropertyDefaultFacet Members

        public object GetDefault(INakedObject inObject) {
            return DefaultValueConst;
        }

        #endregion

        public static bool IsAdaptedType(Type type) {
            return type == typeof (Color);
        }


        protected override Color DoParse(string entry) {
            try {
                int argb;
                if (entry.StartsWith("0x")) {
                    argb = int.Parse(entry.Substring(2), NumberStyles.AllowHexSpecifier);
                }
                else {
                    argb = entry.StartsWith("#") ? int.Parse(entry.Substring(1), NumberStyles.AllowHexSpecifier) : int.Parse(entry);
                }

                return Color.FromArgb(argb);
            }
            catch (FormatException) {
                throw new InvalidEntryException(FormatMessage(entry));
            }
            catch (OverflowException) {
                throw new InvalidEntryException(string.Format(Resources.NakedObjects.OutOfRange, entry, int.MinValue, int.MaxValue));
            }
        }

        protected override Color DoParseInvariant(string entry) {
            return Color.FromArgb(int.Parse(entry, CultureInfo.InvariantCulture));
        }

        protected override string GetInvariantString(Color obj) {
            return obj.ToArgb().ToString(CultureInfo.InvariantCulture);
        }

        protected override string TitleStringWithMask(string mask, Color value) {
            return value.ToString();
        }

        protected override string DoEncode(Color obj) {
            return (obj).ToArgb().ToString();
        }

        protected override Color DoRestore(string data) {
            return Color.FromArgb(int.Parse(data));
        }

        public int ColorValue(INakedObject nakedObject) {
            if (nakedObject == null) {
                return 0;
            }
            var color = (Color) nakedObject.Object;
            return color.ToArgb();
        }


        public override string ToString() {
            return "ColorAdapter: ";
        }
    }
}