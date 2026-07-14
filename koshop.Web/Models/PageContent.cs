using System.ComponentModel.DataAnnotations.Schema;

namespace KoShop.Web.Models;

// These map 1-to-1 onto the flat "single settings row per page" tables you
// already created by hand in SSMS (Tbl_Setting, Tbl_Home, Tbl_About, ...).
// Each table always has exactly one row (Id = 1) that the Page Editor reads/updates.

[Table("Tbl_Setting")]
public class SiteSetting
{
    public int Id { get; set; }
    public string? SiteTitle { get; set; }
    public string? SiteLogo { get; set; }
    public string? HeaderPhone { get; set; }
    public string? FooterDesc1 { get; set; }
    public string? FooterDesc2 { get; set; }
}

[Table("Tbl_Home")]
public class HomePageContent
{
    public int Id { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoMeta { get; set; }
    public string? HeaderTitle { get; set; }   // eyebrow ("به زودی...")
    public string? Titel_Body1 { get; set; }   // hero heading line 1
    public string? Titel_Body2 { get; set; }   // hero heading line 2 (colored)
    public string? Text_Body { get; set; }     // hero description
    public string? Btn1_Text { get; set; }
    public string? Btn2_Text { get; set; }
    public string? ImgSlide1 { get; set; }     // hero image
    public string? Box1_Title { get; set; }    // categories section title
    public string? Box2_Title { get; set; }    // featured products section title
    public string? Box3_Title { get; set; }    // "order steps" section title
    public string? Box3_Text { get; set; }
    public string? Box3_BtnText { get; set; }
    public string? FooterTitle { get; set; }   // newsletter section title
    public string? FooterText { get; set; }
}

[Table("Tbl_About")]
public class AboutPageContent
{
    public int Id { get; set; }
    public string? SeoTitle { get; set; }
    public string? HeaderTitle { get; set; }
    public string? Text_Body { get; set; }
    public string? Box1_Title { get; set; }    // "مزیت‌های ما"
    public string? Box2_Title { get; set; }    // "تیم ما"
    public string? Box3_Title { get; set; }    // "تعهد ما"
    public string? Box3_Text { get; set; }
}

[Table("Tbl_Contact")]
public class ContactPageContent
{
    public int Id { get; set; }
    public string? SeoTitle { get; set; }
    public string? HeaderTitle { get; set; }
    public string? ImgSlide1 { get; set; }
    public string? Box1_Title { get; set; }    // "راه‌های ارتباطی"
    public string? Box2_Title { get; set; }    // "فرم تماس"
    public string? Box3_Title { get; set; }    // "ما را پیدا کنید"
    public string? FooterTitle { get; set; }   // "سوال دارید؟"
}

[Table("Tbl_Faq")]
public class FaqPageContent
{
    public int Id { get; set; }
    public string? SeoTitle { get; set; }
    public string? HeaderTitle { get; set; }
    public string? FooterTitle { get; set; }
}

[Table("Tbl_Terms")]
public class TermsPageContent
{
    public int Id { get; set; }
    public string? SeoTitle { get; set; }
    public string? HeaderTitle { get; set; }
    public string? FooterTitle { get; set; }
}

[Table("Tbl_Privacy")]
public class PrivacyPageContent
{
    public int Id { get; set; }
    public string? SeoTitle { get; set; }
    public string? HeaderTitle { get; set; }
    public string? Section1_Title { get; set; }
    public string? Section2_Title { get; set; }
    public string? Section3_Title { get; set; }
    public string? Section4_Title { get; set; }
    public string? Section5_Title { get; set; }
    public string? Section6_Title { get; set; }
    public string? Section7_Title { get; set; }
}

[Table("Tbl_Shop")]
public class ShopPageContent
{
    public int Id { get; set; }
    public string? SeoTitle { get; set; }
    public string? HeaderTitle { get; set; }
    public string? EmptyStateTitle { get; set; }
}

[Table("Tbl_SubmitOrder")]
public class SubmitOrderPageContent
{
    public int Id { get; set; }
    public string? SeoTitle { get; set; }
    public string? HeaderTitle { get; set; }
}

[Table("Tbl_TrackOrder")]
public class TrackOrderPageContent
{
    public int Id { get; set; }
    public string? SeoTitle { get; set; }
    public string? HeaderTitle { get; set; }
    public string? Box1_Title { get; set; }
}
