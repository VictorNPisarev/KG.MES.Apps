window.dashboardDragDrop =
{
	init: function (dotnetHelper)
	{
		document.addEventListener('dragover', function (e)
		{
			e.preventDefault();
		});

		document.addEventListener('drop', function (e)
		{
			e.preventDefault();
		});

		document.addEventListener('dragstart', function (e)
		{
			const widget = e.target.closest('.dashboard-widget');
			if (widget)
			{
				widget.classList.add('dragging');
			}
		});

		document.addEventListener('dragend', function (e)
		{
			const widget = e.target.closest('.dashboard-widget');
			if (widget)
			{
				widget.classList.remove('dragging');
				document.querySelectorAll('.drag-over').forEach(el => el.classList.remove('drag-over'));
			}
		});

		// Перехватываем dragover/drop на всём документе
		document.addEventListener('dragover', function (e)
		{
			const widget = e.target.closest('.dashboard-widget');
			if (widget)
			{
				document.querySelectorAll('.drag-over').forEach(el =>
				{
					if (el !== widget) el.classList.remove('drag-over');
				});
				widget.classList.add('drag-over');
			}
		});

		document.addEventListener('drop', function (e) {
			e.preventDefault();

			const targetWidget = e.target.closest('.dashboard-widget');
			const draggedWidget = document.querySelector('.dashboard-widget.dragging');
			const targetTabs = e.target.closest('.dashboard-tabs');

			document.querySelectorAll('.drag-over').forEach(el => el.classList.remove('drag-over'));

			if (!draggedWidget) return;

			const draggedId = draggedWidget.dataset.widgetId;

			if (targetTabs && !targetWidget) {
				// Бросили на панель вкладок
				console.log('Drop to tabs:', draggedId);
				dotnetHelper.invokeMethodAsync('OnDropToTabs', draggedId);
			} else if (targetWidget && targetWidget !== draggedWidget) {
				// Бросили на другой виджет — обмен
				const targetId = targetWidget.dataset.widgetId;
				console.log('Drop:', draggedId, '->', targetId);
				dotnetHelper.invokeMethodAsync('OnWidgetDrop', draggedId, targetId);
			}
		});
	},

	getDraggedId: function ()
	{
		const dragged = document.querySelector('.dashboard-widget.dragging');
		return dragged ? dragged.dataset.widgetId : '';
	}
};