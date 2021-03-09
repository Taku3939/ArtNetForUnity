/*!
 * Copyright (c) 2021 Takuya Isaki
 *
 * Released under the MIT license.
 * see https://opensource.org/licenses/MIT
 */

namespace ArtNet
{
    public interface IRecordable
    {
        /// <summary>
        /// 録画するプロパティを返す
        /// </summary>
        /// <returns></returns>
        string[] GetProperty();
    }
}