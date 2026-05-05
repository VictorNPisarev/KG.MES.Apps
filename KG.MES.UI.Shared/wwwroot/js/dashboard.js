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

		document.addEventListener('drop', function (e)
		{
			const targetWidget = e.target.closest('.dashboard-widget');
			const draggedWidget = document.querySelector('.dashboard-widget.dragging');

			document.querySelectorAll('.drag-over').forEach(el => el.classList.remove('drag-over'));

			if (targetWidget && draggedWidget && targetWidget !== draggedWidget)
			{
				const targetId = targetWidget.dataset.widgetId;
				const draggedId = draggedWidget.dataset.widgetId;
				console.log('Drop:', draggedId, '->', targetId);
				dotnetHelper.invokeMethodAsync('OnWidgetDrop', draggedId, targetId);
			}
		});
	}
};