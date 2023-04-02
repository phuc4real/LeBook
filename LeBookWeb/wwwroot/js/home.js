const prevIcon = '<i class="fa-solid fa-chevron-left"></i>'
const nextIcon = '<i class="fa-solid fa-chevron-right"></i>'

$('.owl-banner').owlCarousel({
    loop: true,
    margin: 10,
    nav: true,
    dots: true,
    lazyLoad: true,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    navText: [prevIcon, nextIcon],
    responsive: {
        0: {
            items: 1,
        },
    },
})

const Options = {
    loop: false,
    dots: false,
    nav: true,
    lazyLoad: true,
    margin: 10,
    navText: [prevIcon, nextIcon],
    responsive: {
        0: {
            items: 1,
        },
        600: {
            items: 3,
        },
        900: {
            items: 5,
        },
    },
}

const owl2 = $('.owl-product-2').owlCarousel(Options);
const owl3 = $('.owl-product-3').owlCarousel(Options);
const owl4 = $('.owl-product-4').owlCarousel(Options);

$("document").ready(function () {
    owl2.owlcarousel2_filter('.sachmoi');
    owl3.owlcarousel2_filter('.mangamoi');
    owl4.owlcarousel2_filter('.sachtl');
});

function handleFilterClick($bar, $carousel, girdslider, filterAttr) {
    $bar.on('click', girdslider, function () {
        const $item = $(this);
        $item.addClass('active').siblings().removeClass('active');
        const filter = $item.data(filterAttr);

        $carousel.owlcarousel2_filter(filter);
    });
}

handleFilterClick($('.owl-filter-bar'), owl2, '.girdslider-item','owl-filter');
handleFilterClick($('.owl-filter-bar-2'), owl3, '.girdslider-item-2', 'owl-filter-2');
handleFilterClick($('.owl-filter-bar-3'), owl4, '.girdslider-item-3', 'owl-filter-3');