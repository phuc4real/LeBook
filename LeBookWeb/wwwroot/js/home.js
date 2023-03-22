const prevIcon = '<i class="fa-solid fa-chevron-left"></i>'
const nextIcon = '<i class="fa-solid fa-chevron-right"></i>'

$('.owl-banner').owlCarousel({
    loop: false,
    margin: 10,
    nav: true,
    dots: true,
    lazyLoad: true,
    smartSpeed: 700,
    navText: [prevIcon, nextIcon],
    responsive: {
        0: {
            items: 1,
        },
    },
})

var owl = $('.owl-product-2').owlCarousel({
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
})

var owl3 = $('.owl-product-3').owlCarousel({
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
})

var owl4 = $('.owl-product-4').owlCarousel({
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
})

$("document").ready(function () {
    owl.owlcarousel2_filter('.sachmoi');
    owl3.owlcarousel2_filter('.mangamoi');
    owl4.owlcarousel2_filter('.sachtl');
});

$('.owl-filter-bar').on('click', '.girdslider-item', function () {
    const $item = $(this)
    $item.addClass('active').siblings().removeClass('active')
    const filter = $item.data('owl-filter')

    owl.owlcarousel2_filter(filter)
})

$('.owl-filter-bar-2').on('click', '.girdslider-item-2', function () {
    const $item = $(this)
    $item.addClass('active').siblings().removeClass('active')
    const filter = $item.data('owl-filter-2')

    owl3.owlcarousel2_filter(filter)
})

$('.owl-filter-bar-3').on('click', '.girdslider-item-3', function () {
    const $item = $(this)
    $item.addClass('active').siblings().removeClass('active')
    const filter = $item.data('owl-filter-3')

    owl4.owlcarousel2_filter(filter)
})