function ItemPath() {
    this.left = 0;
    this.width = 0;
}

op = {
    width: 0,
    height: 0
}

ItemsSlide = {
    item: [],
    selected: 0
}

function endRequestHandler() {
    op.height = $('.container-move-path').height();
    op.width = $('.container-move-path').width();

    if (ItemsSlide.item.length == 0) {
        $('.content-move[itemid]').each(function (index, Element) {
            it = new ItemPath();
            it.left = (op.width - $(this).width()) / 2;
            it.width = $(this).width();
            ItemsSlide.item.push(it);
        });
    }

    if (ItemsSlide.item.length > 0) {
        itemSel = ItemsSlide.item[ItemsSlide.selected];
        $('.content-move[itemid=' + ItemsSlide.selected + ']').css({ 'left': itemSel.left + 'px' });
    }

    $('.next').click(function () {
        if (ItemsSlide.selected > 0) {
            if ((parseInt(ItemsSlide.selected) + 1) <= (ItemsSlide.item.length - 1)) {
                itemGoOut = ItemsSlide.selected;
                ItemsSlide.selected += 1;
                it2 = ItemsSlide.item[ItemsSlide.selected];

                $('.content-move[itemid=' + itemGoOut + ']').css({ 'z-index': '999' });
                $('.content-move[itemid=' + ItemsSlide.selected + ']').css({ 'z-index': '1000' });
                $('.content-move[itemid=' + ItemsSlide.selected + ']').stop().animate({ left: it2.left + 'px' }, 500, function () {
                    $('.content-move[itemid=' + itemGoOut + ']').css({ 'left': -(it.width + (it.left)) + 'px' });
                });
            }
        }
        else {
            ItemsSlide.selected += 1;
            $('.content-move[itemid=' + ItemsSlide.selected + ']').stop().animate({ left: ItemsSlide.item[ItemsSlide.selected].left + 'px' }, 500, function () { });
        }
    });

    $('.previous').click(function () {
        if (ItemsSlide.selected >= 1) {
            it = ItemsSlide.item[ItemsSlide.selected];
            $('.content-move[itemid=' + ItemsSlide.selected + ']').stop().animate({ left: -(it.width + (it.left)) + 'px' }, 500, function () { });

            itemGoOut = ItemsSlide.selected;
            ItemsSlide.selected -= 1;
            it2 = ItemsSlide.item[ItemsSlide.selected];
            $('.content-move[itemid=' + itemGoOut + ']').css({ 'z-index': '1000' });
            $('.content-move[itemid=' + ItemsSlide.selected + ']').css({ 'z-index': '999', 'left': it2.left + 'px' });
        }
    });

    $('.sheetIndex > a').click(function () {
        itemGoOut = ItemsSlide.selected;
        ItemsSlide.selected = parseInt($(this).attr("itemid"));

        it2 = ItemsSlide.item[ItemsSlide.selected];

        if (itemGoOut == ItemsSlide.selected) return; /* si es el mismo elemento mostrado entonces no hacemos nada */
        if (ItemsSlide.selected > itemGoOut) {
            $('.content-move[itemid=' + itemGoOut + ']').css({ 'z-index': '999' }); /* El viejo deja de visualizarse por lo que ahora vamos tiene que estar una capa más abajo que el próximo */
            $('.content-move[itemid=' + ItemsSlide.selected + ']').css({ 'z-index': '1000' });

            $('.content-move[itemid=' + ItemsSlide.selected + ']').stop().animate({ left: it2.left + 'px' }, 500, function () {
                $('.content-move[itemid=' + itemGoOut + ']').css({ 'left': -(it2.width + (it2.left)) + 'px' });
            });
        }
        else {
            $('.content-move[itemid=' + itemGoOut + ']').css({ 'z-index': '1000' }); /* El viejo deja de visualizarse por lo que ahora vamos tiene que estar una capa más abajo que el próximo */
            $('.content-move[itemid=' + ItemsSlide.selected + ']').css({ 'z-index': '999' });

            $('.content-move[itemid=' + ItemsSlide.selected + ']').css({ left: it2.left + 'px' });
            $('.content-move[itemid=' + itemGoOut + ']').stop().animate({ left: -(it2.width + (it2.left)) + 'px' }, 500, function () { });
        }
    });
}