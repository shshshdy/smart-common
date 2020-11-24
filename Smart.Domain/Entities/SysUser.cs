using Smart.Infrastructure.Freesql.Entities;
using Smart.Domain.Entities.Enums;
using FreeSql.DataAnnotations;

namespace Smart.Domain.Entities
{
    public class SysUser : EntityBase<long>,IEntitySoftDelete
    {

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 真实名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 汉语全拼
        /// </summary>
        public string Pinyin { get; set; }

        /// <summary>
        /// 拼音首字母
        /// </summary>
        public string ShortPinyin { get; set; }
        /// <summary>
        /// e-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Column(MapType = typeof(int))]
        public Sex Sex { get; set; }
    }
}
