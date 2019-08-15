(function($) {
  
  "use strict";  

  $(window).on('load', function() {

    /* 
   MixitUp
   ========================================================================== */
  $('#portfolio').mixItUp();

  /* 
   One Page Navigation & wow js
   ========================================================================== */
    var OnePNav = $('.onepage-nev');
    var top_offset = OnePNav.height() - -0;
    OnePNav.onePageNav({
      currentClass: 'active',
      scrollOffset: top_offset,
    });
  
  /*Page Loader active
    ========================================================*/
    $('#preloader').fadeOut();

  // Sticky Nav
    $(window).on('scroll', function() {
        if ($(window).scrollTop() > 200) {
            $('.scrolling-navbar').addClass('top-nav-collapse');
        } else {
            $('.scrolling-navbar').removeClass('top-nav-collapse');
        }
    });

    /* slicknav mobile menu active  */
    $('.mobile-menu').slicknav({
        prependTo: '.navbar-header',
        parentTag: 'liner',
        allowParentLinks: true,
        duplicate: true,
        label: '',
        closedSymbol: '<i class="icon-arrow-right"></i>',
        openedSymbol: '<i class="icon-arrow-down"></i>',
      });

      /* WOW Scroll Spy
    ========================================================*/
     var wow = new WOW({
      //disabled for mobile
        mobile: false
    });

    wow.init();

    ///* Nivo Lightbox 
    //========================================================*/
    //$('.lightbox').nivoLightbox({
    //    effect: 'fadeScale',
    //    keyboardNav: true,
    //  });

    /* Counter
    ========================================================*/
    $('.counterUp').counterUp({
     delay: 10,
     time: 1000
    });


    /* Back Top Link active
    ========================================================*/
      var offset = 200;
      var duration = 500;
      $(window).scroll(function() {
        if ($(this).scrollTop() > offset) {
          $('.back-to-top').fadeIn(400);
        } else {
          $('.back-to-top').fadeOut(400);
        }
      });

      $('.back-to-top').on('click',function(event) {
        event.preventDefault();
        $('html, body').animate({
          scrollTop: 0
        }, 600);
        return false;
      });
  });      

}(jQuery));

const openImageModal = (id, imagesSource, title = 'modal title', description = 'modal description') => {

    let modalEl = document.createElement('div');
    modalEl.setAttribute('id', id);
    modalEl.setAttribute('class', 'modal fade mt-5');  

    let modalDialogEl = document.createElement('div');
    modalDialogEl.className = 'modal-dialog modal-lg modal-dialog-centered ';

    let modalContentEl = document.createElement('div');
    modalContentEl.className = 'modal-content';

    let modalBody = document.createElement('div');
    modalBody.className = 'modal-body';

    let modalFooter = document.createElement('div');
    modalFooter.className = 'modal-footer';
    modalFooter.innerHTML = `<button type='button' class='btn btn-danger' data-dismiss='modal'>Close</button>`;  

    // Carousel
    let carouselEl = document.createElement('div');
    carouselEl.setAttribute('id', `${id}_carousel`);
    carouselEl.setAttribute('class', 'carousel slide');
    carouselEl.setAttribute('data-ride', 'carousel');

    let carouselIndicators = document.createElement('ul');
    carouselIndicators.className = 'carousel-indicators';
    carouselIndicators.innerHTML = `<li data-target='#${id}_carousel' data-slide-to='0' class='active'></li>`;

    for (let i = 1; i < imagesSource.length; i++) {
        carouselIndicators.insertAdjacentHTML('beforeend', `<li data-target='#${id}_carousel' data-slide-to='${i}'></li>`);
    }

    let carouselInnerEl = document.createElement('div');
    carouselInnerEl.className = 'carousel-inner';
    let isFirstItem = true;

    for (let i = 0; i < imagesSource.length; i++) {
        if (isFirstItem) {
            carouselInnerEl.insertAdjacentHTML('beforeend', `<div class='carousel-item active'><img class='img-fluid w-100' src='${imagesSource[i]}' alt='image'></div>`);
            isFirstItem = false;
        }
        else {
            carouselInnerEl.insertAdjacentHTML('beforeend', `<div class='carousel-item'><img class='img-fluid w-100' src='${imagesSource[i]}' alt='image'></div>`);
        }
    }

    carouselEl.append(carouselIndicators);
    carouselEl.append(carouselInnerEl);
    carouselEl.insertAdjacentHTML('beforeend', `<a class='carousel-control-prev' href='#${id}_carousel' data-slide='prev'><span class='carousel-control-prev-icon'></span></a>`);
    carouselEl.insertAdjacentHTML('beforeend', `<a class='carousel-control-next' href='#${id}_carousel' data-slide='next'><span class='carousel-control-next-icon'></span></a>`);

    modalEl.append(modalDialogEl);
    modalDialogEl.append(modalContentEl);  
    modalContentEl.append(carouselEl);
    modalContentEl.append(modalBody);
    modalContentEl.append(modalFooter);

    modalBody.insertAdjacentHTML('beforeend', `<h5 class='my-2'>${title}</h5><hr />`);
    modalBody.insertAdjacentHTML('beforeend', `<p>${description}</p>`);

    let modalWrapperEl = document.getElementById('modal-wrapper');
    modalWrapperEl.replaceChild(modalEl, modalWrapperEl.firstChild);
}