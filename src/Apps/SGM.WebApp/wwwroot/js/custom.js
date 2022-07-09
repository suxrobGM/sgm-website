/*-----------------------------------------------------------
* Template Name    : Kerri | Responsive Bootstrap 5 Personal Template
* Author           : SRBThemes
* Created          : March 2018
* File Description : Main Js file of the template
*------------------------------------------------------------
*/

$("#status").fadeOut(),$("#preloader").delay(350).fadeOut("slow"),$("body").delay(350).css({overflow:"visible"});var scrollSpy=new bootstrap.ScrollSpy(document.body,{target:"#main_nav",offset:70}),navbar=document.querySelector("nav");window.onscroll=function(){window.pageYOffset>200?navbar.classList.add("stickyadd"):navbar.classList.remove("stickyadd")},$(".owl-carousel").owlCarousel({loop:!0,nav:!1,items:1,autoplay:!0,autoplayTimeout:5000,autoplayHoverPause:!0,autoHeight:!1,autoHeightClass:"owl-height"}),$(window).on("load",function(){var b=$(".work-filter"),c=$("#menu-filter");b.isotope({filter:"*",layoutMode:"masonry",animationOptions:{duration:750,easing:"linear"}}),c.find("a").on("click",function(){var a=$(this).attr("data-filter");return c.find("a").removeClass("active"),$(this).addClass("active"),b.isotope({filter:a,animationOptions:{animationDuration:750,easing:"linear",queue:!1}}),!1})}),$(".img-zoom").magnificPopup({type:"image",closeOnContentClick:!0,mainClass:"mfp-fade",gallery:{enabled:!0,navigateByImgClick:!0,preload:[0,1]}}),$(window).on("scroll",function(){$(this).scrollTop()>100?$(".back_top").fadeIn():$(".back_top").fadeOut()}),$(".back_top").click(function(){return $("html, body").animate({scrollTop:0},1000),!1});