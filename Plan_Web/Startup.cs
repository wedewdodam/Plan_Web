using Blazored.LocalStorage;
using Facility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Plan_Apt_Lib;
using Plan_Blazor_Lib;
using Plan_Blazor_Lib.Cost;
using Plan_Blazor_Lib.Cycle;
using Plan_Blazor_Lib.Note_View;
using Plan_Blazor_Lib.Plan;
using Plan_Blazor_Lib.Price;
using Plan_Blazor_Lib.Record;
using Plan_Blazor_Lib.Review;
using Plan_Lib;
using Plan_Lib.Company;
using Plan_Lib.Logs;
using Plan_Lib.Pund;
using Plan_Web.Areas.Identity;
using Plan_Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromMinutes(60);
            //});

            services.AddHttpContextAccessor(); //[1]

            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(
                     Configuration.GetConnectionString("Khmais_db_Connection")));

            services.AddBlazoredLocalStorage(); //���� ���

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews(); //Mvc
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpClient();

            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddSingleton<WeatherForecastService>();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddRazorPages().AddSessionStateTempDataProvider();

            //services.AddMvc(); //TODO: DO I NEED IT?
            services.AddDistributedMemoryCache();  //TODO : DO I NEED IT? // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();

            // ��Ű ���� ���
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            // ��� Dapper �� �����
            Class_lib(services);
        }

        private void Class_lib(IServiceCollection services)
        {
            services.AddTransient<IkhmaInfor_Lib, khmaInfor_Lib>();

            services.AddTransient<IFacility_Lib, Facility_Lib>();
            services.AddTransient<IFacility_Sort_Lib, Facility_Sort_Lib>();
            services.AddTransient<IFacility_Detail_Lib, Facility_Detail_Lib>();

            services.AddTransient<IAdditional_Welfare_Facility_Lib, Additional_Welfare_Facility_Lib>();//�����ü� ��

            services.AddTransient<IAptInfor_Lib, AptInfor_Lib>();
            services.AddTransient<IArticle_Lib, Article_Lib>();
            services.AddTransient<ILogView, LogView>();
            services.AddTransient<IStaff, Staff>(); //��� �������� ������ ����
            services.AddTransient<IAptInfor_Lib, AptInfor_Lib>(); //��� �������� �⺻����
            services.AddTransient<IApt_Detail_Lib, Apt_Detail_Lib>(); //��� �������� ������

            services.AddTransient<IRepair_Plan_Lib, Repair_Plan_Lib>(); //�������� ��ȹ����
            services.AddTransient<IPlan_Review_Lib, Plan_Review_Lib>(); // ��������ȹ �ѷ� ����
            services.AddTransient<IReview_Content_Lib, Review_Content_Lib>();//��������ȹ �׸� ����
            services.AddTransient<IKhma_Note_Lib, Khma_Note_Lib>(); // �Խ��� ���� Ŭ����
            services.AddTransient<ISort_Note_Lib, Sort_Note_Lib>(); //�Խ��� �з� Ŭ����
            services.AddTransient<IKhma_NoteComments_Lib, Khma_NoteComments_Lib>(); //�Խ��� ��� Ŭ����
            services.AddTransient<ICycle_Lib, Cycle_Lib>(); //��� �����ֱ� Ŭ����
            services.AddTransient<ICost_Lib, Cost_Lib>(); //��� �����ݾ� Ŭ����

            services.AddTransient<IRepair_Capital_Lib, Repair_Capital_Lib>();//��� ���� ���� Ŭ����
            services.AddTransient<ICapital_Levy_Lib, Capital_Levy_Lib>(); //������ ���� ¡�� ���� Ŭ����
            services.AddTransient<IRepair_Capital_Use_Lib, Repair_Capital_Use_Lib>();//���������� ��� �������̽�

            services.AddTransient<IRepair_Record_Lib, Repair_Record_Lib>(); //������ �����̷� ���� Ŭ����

            services.AddTransient<ICompany_Lib, Company_Lib>(); //��ü ����
            services.AddTransient<ICompany_Etc_Lib, Company_Etc_Lib>();// ��ü �� ����
            services.AddTransient<ICompanySort_Lib, CompanySort_Lib>();// ��ü �з� ����
            services.AddTransient<IBusinessType_Lib, BusinessType_Lib>();// ���� �з� ����
            services.AddTransient<IBusiList_Lib, BusiList_Lib>(); //���� ���� �з�
            services.AddTransient<IApt_Company_Lib, Apt_Company_Lib>(); //�������ÿ��� ������ ��ü ����

            services.AddTransient<IDong_Lib, Dong_Lib>(); //������
            services.AddTransient<IDong_Composition, Dong_Composition>();//����������

            services.AddTransient<IRelation_Law_Lib, Relation_Law_Lib>(); //���� ���� ����
            services.AddTransient<IBylaw_Lib, Bylaw_Lib>();//�����Ծ� ����
            services.AddTransient<IUnit_Price_Lib, Unit_Price_Lib>();

            services.AddTransient<IRepair_SmallSum_Object_Lib, Repair_SmallSum_Object_Lib>(); //�Ҿ����� ���
            services.AddTransient<IRepair_SmallSum_Requirement_Lib, Repair_SmallSum_Requirement_Lib>(); //�Ҿ����� ���
            services.AddTransient<IRepair_Object_Selection_Lib, Repair_Object_Selection_Lib>(); //�Ҿ״�� ����
            services.AddTransient<ISmallSum_Repuirement_Selection_Lib, SmallSum_Requirement_Selection_Lib>();//�Ҿ׿�� ����
            services.AddTransient<ILogView_Lib, LogView_Lib>();//�α� �����

            services.AddTransient<IRepair_DetailKind_Lib, Repair_DetailKind_Lib>(); //�ܰ� ǰ�� ����
            services.AddTransient<IPrice_Set_Lib, Price_Set_Lib>(); // �����ݾ� �Է� �ܰ� ���� ����
            services.AddTransient<IRepair_Price_Lib, Repair_Price_Lib>();  //�ܰ�����

            services.AddTransient<ICost_Using_Plan_Lib, Cost_Using_Plan_Lib>(); // ����ȹ�� ���� ����
            services.AddTransient<IUnitPrice_Rate_Lib, UnitPrice_Rate_Lib>(); // ������꼭 ������ ���� �Է�
            services.AddTransient<IPrime_Cost_Report_Lib, Prime_Cost_Report_Lib>(); // ������꼭 ���� �ۼ� �޼���
            services.AddTransient<IPlan_Prosess_Lib, Plan_Prosess_Lib>();//��������ȹ ���� ����
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
                endpoints.MapRazorPages();

                //endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");

                //MVC�� Ȩ���� ��� �� ����
                //endpoints.MapGet("/", context =>
                //{
                //    context.Response.Redirect("/Home");
                //    return Task.CompletedTask;
                //});
            });
        }
    }
}
